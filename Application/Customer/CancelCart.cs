using Application.Cart;
using Application.Interfaces;
using FluentResults;
using GenericRepository;
using MediatR;
using IUnitOfWork = Application.Interfaces.IUnitOfWork;

namespace Application.Customer;

public class CancelCart
{
    public class CancelCartRequest : IRequest<Result>
    {
        public int CartId { get; set; } 

    }
    public class CancelCartRequestHandler(ICartRepository iCartRepository,IUnitOfWork iUnitOfWork):IRequestHandler<CancelCartRequest,Result>
    {
        public async Task<Result> Handle(CancelCartRequest request, CancellationToken cancellationToken)
        {
            if (!await iCartRepository.AnyAsync(x=>x.Id==request.CartId, cancellationToken))
            {
                return Result.Fail("Cart is not found!");
            }
            var cart =await iCartRepository.FirstOrDefaultAsync(x=>x.Id==request.CartId,cancellationToken);
            
            if (cart.OrderStatus != nameof(CreateOrderCommand.OrderStatus.CourierPending))
            {
                return Result.Fail("Cart status is not correct! !");
            }

            cart.OrderStatus = nameof(CreateOrderCommand.OrderStatus.Cancelled);
            cart.IsDelete = true;
            cart.DeleteAt=DateTime.UtcNow;
            iCartRepository.Update(cart);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();

        }
        

    }
}