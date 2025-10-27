using Application.Interfaces;
using Application.Product;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Cart;

public class DeleteByProductName
{
    public class DeleteByProductNameRequest():IRequest<Result>
    {
        public string ProductName { get; set; } = default!;
    }

    public class DeleteByProductNameRequestHandler(IUnitOfWork iUnitOfWork,ICartRepository iCartRepository,IHttpContextAccessor httpContextAccessor,IProductRepository iProductRepository):IRequestHandler<DeleteByProductNameRequest,Result>
    {
        public async Task<Result> Handle(DeleteByProductNameRequest request, CancellationToken cancellationToken)
        {
            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");
            // if (!await iProductRepository.AnyAsync(x=>x.Name==request.ProductName,cancellationToken))
            // {
            //     return Result.Fail("Product is not found!");
            // }

            var product =await iProductRepository.FirstOrDefaultAsync(x => x.Name == request.ProductName, cancellationToken);

            var cart = await iCartRepository.FirstOrDefaultAsync(x=>x.PersonFin==personFin && x.ProductId==product.Id, cancellationToken);
            if (cart is null)
            {
                return Result.Fail("Cart is not found!");
            }
            if (personFin != cart.PersonFin)
            {
                return Result.Fail("Unauthorized access !");

            }

            cart.IsDelete = true;
            cart.DeleteAt=DateTime.UtcNow;
            iCartRepository.Update(cart);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();


        }
    }
}