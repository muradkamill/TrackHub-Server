using Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Cart;

public class ChangeSelectCartCommand
{
    public class ChangeSelectCartCommandRequest : IRequest<Result>
    {
        public int CartId { get; set; }
    }
    public class NoSelectCartCommandRequestHandler(ICartRepository iCartRepository,IUnitOfWork iUnitOfWork,IHttpContextAccessor httpContextAccessor):IRequestHandler<ChangeSelectCartCommandRequest,Result>
    {
        public async Task<Result> Handle(ChangeSelectCartCommandRequest request, CancellationToken cancellationToken)
        {
            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");
            
            
            var cart = await iCartRepository.FirstOrDefaultAsync(x => x.Id == request.CartId, cancellationToken);
            if (cart is null)
            {
                return Result.Fail("Cart is not found!");
            }

            if (cart.PersonFin !=personFin)
            {
                return Result.Fail("Unauthorized access !");
            }

            if (cart.IsSelected)
            {
                cart.IsSelected = false;
            }
            else
            {
                cart.IsSelected = true;
            }

            iCartRepository.Update(cart);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}