using Application.Cart;
using Application.Interfaces;
using Application.Person;
using Domain.Comment;
using FluentResults;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Customer;

public class RateCourier
{
    public static List<decimal> RateList =
    [
        0,
        0.5m,
        1,
        1.5m,
        2,
        2.5m,
        3,
        3.5m,
        4,
        4.5m,
        5
    ];
    public class RateCourierRequestValidation : AbstractValidator<RateCourierRequest>
    {
        public RateCourierRequestValidation()
        {
            RuleFor(x => x.CourierRate).Must(x => RateList.Contains(x));
        }
    }
    public class RateCourierRequest : IRequest<Result>
    {
        public decimal CourierRate { get; set; }
        public int CartId { get; set; }
    } 
    public class RateCourierRequestHandler(IPersonRepository iPersonRepository,IUnitOfWork iUnitOfWork, ICartRepository iCartRepository):IRequestHandler<RateCourierRequest,Result>
    {
        public async Task<Result> Handle(RateCourierRequest request, CancellationToken cancellationToken)
        {
            var cart = await iCartRepository.FirstOrDefaultAsync(x=>x.Id==request.CartId, cancellationToken);
            if (cart is null)
            {
                return Result.Fail("Delivered order is not found");
            }

            if (cart.OrderStatus != nameof(CreateOrderCommand.OrderStatus.Delivered))
            {
                return Result.Fail("Package is not delivered!");

            }
            var courier = await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==cart.CourierFin, cancellationToken);
            var totalRate = (courier.CourierRate*(courier.DeliveredPackageQuantity-1) + request.CourierRate) / courier.DeliveredPackageQuantity;
            
            
            courier.CourierRate = totalRate;
            iPersonRepository.Update(courier);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}