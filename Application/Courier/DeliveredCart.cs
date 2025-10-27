using System.Net;
using System.Net.Mail;
using Application.Auth;
using Application.Cart;
using Application.Interfaces;
using Application.Person;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace Application.Courier;

public class DeliveredCart
{
    public class DeliveredPackageRequest : IRequest<Result>
    {
        public int CartId { get; set; } 
    }
    public class DeliveredPackageRequestHandler(IHubContext<NotificationHub> iHubContext,ICartRepository iCartRepository,IUnitOfWork iUnitOfWork, IPersonRepository iPersonRepository,IHttpContextAccessor httpContextAccessor,IConfiguration configuration):IRequestHandler<DeliveredPackageRequest,Result>
    {
        public async Task<Result> Handle(DeliveredPackageRequest request, CancellationToken cancellationToken)
        {
            var courierFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(courierFin))
                return Result.Fail("Unauthorized access !");
            if (!await iCartRepository.AnyAsync(x=>x.Id==request.CartId,cancellationToken))
            {
                return Result.Fail("Cart is not found!");
            }

            var cart = await iCartRepository.FirstOrDefaultAsync(x => x.Id == request.CartId, cancellationToken);
            if (cart.OrderStatus!=nameof(CreateOrderCommand.OrderStatus.CourierAccept))
            {
                return Result.Fail("Order status is not correct!");
            }

            cart.OrderStatus = nameof(CreateOrderCommand.OrderStatus.Delivered);
            var courier =await iPersonRepository.FirstOrDefaultAsync(x => x.Fin == cart.CourierFin, cancellationToken);
            var owner=await iPersonRepository.FirstOrDefaultAsync(x => x.Fin == cart.PersonFin, cancellationToken);
            await iHubContext.Clients.User(cart.PersonFin).SendAsync("RateCourierPopup",new
            {
                message="Please rate courier!",
                courierFin=cart.CourierFin,
                cartId=cart.Id
            }, cancellationToken);
            cart.DeliveredDate=DateTime.UtcNow;
            courier.Balance += cart.Price;
            iCartRepository.Update(cart);
            courier.DeliveredPackageQuantity += 1;
            iPersonRepository.Update(courier);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);

            var accessToken = Methods.CreateAccessToken(configuration,owner);

            string message = $@"
<html>
<body>
    <p>Hello,</p>
    <p>Your order has been delivered. Please rate your product.</p>
    <p>
        <a href='http://localhost:4200/rate/{cart.ProductId}/{accessToken}' 
           style='display:inline-block; padding:10px 20px; color:#fff; background-color:#007bff; text-decoration:none; border-radius:5px;'>
           Rate Product
        </a>
    </p>
</body>
</html>";;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("muradkamilllll@gmail.com", "dwwy jofy pkzv fhyy\n\n"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("muradkamilllll@gmail.com"),
                Subject = "Your Order Has Been Delivered – Rate Your Product",
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(owner.Mail);

            await smtpClient.SendMailAsync(mailMessage,cancellationToken);
            return Result.Ok();
        }
    }
}