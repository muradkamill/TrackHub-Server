using System.Globalization;
using Application.Interfaces;
using Application.Person;
using Application.Product;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Cart;

public class CreateOrderCommand
{
    public class CreateOrderCommandValidation : AbstractValidator<CreateOrderCommandRequest>
    {
        public CreateOrderCommandValidation()
        {
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -90 and 90");

        }
    }
    public enum OrderStatus
    {
        NotOrdered,
        CourierPending,
        CourierAccept,
        Delivered,  
        Cancelled
    }
    public enum CourierStatus
    {
        Pending
    }
    public class CreateOrderCommandRequest():IRequest<Result>
    {
        public string VehicleType { get; set; } = default!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CreateOrderCommandRequestHandler(IProductRepository iProductRepository,IPersonRepository iPersonRepository,ICartRepository iCartRepository,IUnitOfWork iUnitOfWork,IHttpContextAccessor httpContextAccessor):IRequestHandler<CreateOrderCommandRequest,Result>
    {
        public async Task<Result> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var personFin = httpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access!");
            // var personFin = "2222222";
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.VehicleType = textInfo.ToTitleCase(request.VehicleType.ToLowerInvariant());
            if (!Enum.IsDefined(typeof(CreateCourierApplication.VehicleTypeEnum),request.VehicleType))
            {
                return Result.Fail("Vehicle type is not available!");

            }
            if (!await iCartRepository.AnyAsync(x=>x.PersonFin==personFin,cancellationToken))
            {
                return Result.Fail("Cart is not found!");
            }
            var carts =iCartRepository.Where(x => x.PersonFin == personFin && x.IsSelected==true ).AsNoTracking().ToList();
            foreach (var cart in carts)
            {
                if (!await iPersonRepository.AnyAsync(x => x.Fin ==personFin, cancellationToken))
                {
                    return Result.Fail("Product is not found!");

                }
                var product = await iProductRepository.FirstOrDefaultAsync(x => x.Id == cart.ProductId, cancellationToken);
                if (product == null)
                {
                    continue;
                }
                var price = product.Price * cart.Quantity;
                var person = await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==product.OwnerFin, cancellationToken);
                person.Balance += price;
                cart.OrderStatus = nameof(OrderStatus.CourierPending);
                cart.Longitude = request.Longitude;
                cart.Latitude = request.Latitude;
                cart.VehicleType = request.VehicleType;
                cart.CourierFin = nameof(CourierStatus.Pending);
                iCartRepository.Update(cart);
            }
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Ok();
            
            
        }
    }
}