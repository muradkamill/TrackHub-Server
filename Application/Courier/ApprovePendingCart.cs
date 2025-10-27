using Application.Cart;
using Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Courier;

public class ApprovePendingCart
{
    public class ApprovePendingPackageRequest : IRequest<Result>
    {
        public int CartId { get; set; }
        public decimal CourierFee { get; set; }

    }  
    public class ApprovePendingPackageRequestHandler(ICartRepository iCartRepository,IUnitOfWork iUnitOfWork, IHttpContextAccessor httpContextAccessor):IRequestHandler<ApprovePendingPackageRequest,Result>
    {
        public async Task<Result> Handle(ApprovePendingPackageRequest request, CancellationToken cancellationToken)
        {
            var courierFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(courierFin))
                return Result.Fail("Unauthorized access !");
            if (!await iCartRepository.AnyAsync(x=>x.Id==request.CartId,cancellationToken))
            {
                return Result.Fail("Cart is not found!");
            }

            var cart = await iCartRepository.FirstOrDefaultAsync(x => x.Id == request.CartId, cancellationToken);
            cart.CourierFee =request.CourierFee ;
            cart.CourierFin =courierFin ;
            cart.OrderStatus = nameof(CreateOrderCommand.OrderStatus.CourierAccept);
            iCartRepository.Update(cart);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();




        }
    }
}