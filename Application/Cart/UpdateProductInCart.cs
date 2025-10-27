using System.Globalization;
using Application.Interfaces;
using Application.Person;
using Application.Product;
using Domain.Card;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Cart;

public class UpdateProductInCart
{
    public class UpdateProductInCartRequest() : IRequest<Result>
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }    
    }
    
    public class UpdateProductInCartRequestHandler(IProductRepository iProductRepository,IUnitOfWork iUnitOfWork,ICartRepository iCartRepository) :IRequestHandler<UpdateProductInCartRequest,Result>
    {
        public async Task<Result> Handle(UpdateProductInCartRequest request, CancellationToken cancellationToken)
        {
            if (!await iCartRepository.AnyAsync(x=>x.Id==request.CartId,cancellationToken))
            {
                return Result.Fail("Cart is not found!");

            }
            var updateThisCart = await iCartRepository.FirstOrDefaultAsync(x=>x.Id==request.CartId, cancellationToken);
            if (updateThisCart.OrderStatus!=nameof(CreateOrderCommand.OrderStatus.NotOrdered) )
            {
                return Result.Fail("Cart status is not available!");

            }
            var product = await iProductRepository.FirstOrDefaultAsync(y=>y.Id==updateThisCart.ProductId, cancellationToken);
            if (product.StockQuantity<request.Quantity)
            {
                return Result.Fail("This quantity is not available!");
            }

            var cart = request.Adapt(updateThisCart);
            cart.Price = cart.Quantity * product.Price;
            iCartRepository.Update(cart);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}