using Application.Interfaces;
using Application.Person;
using Application.Product;
using Domain.Card;
using FluentResults;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Application.Cart;

public class CreateProductInCart
{
    public class CreateProductInCardValidation:AbstractValidator<CreateProductInCardRequest>
    {
        public CreateProductInCardValidation()
        {
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1).WithMessage("Quantity must be minimum 1");
        }
    }
    public class CreateProductInCardRequest():IRequest<Result>
    {
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
    }

    public class CreateProductInCardRequestHandler(ICartRepository iCartRepository,IProductRepository iProductRepository,IUnitOfWork iUnitOfWork,IHttpContextAccessor httpContextAccessor) :IRequestHandler<CreateProductInCardRequest,Result>
    {
        public async Task<Result> Handle(CreateProductInCardRequest request, CancellationToken cancellationToken)
        {
            var personFin =httpContextAccessor.HttpContext!.User.FindFirst("fin")!.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");


            if (!await iProductRepository.AnyAsync(x=>x.Name==request.ProductName,cancellationToken))
            {
                return Result.Fail("Product is not found!");

            }
            var product = await iProductRepository.FirstOrDefaultAsync(x=>x.Name==request.ProductName, cancellationToken);
            var stockQuantity = product.StockQuantity;
            
            if (stockQuantity<request.Quantity)
            {
                return Result.Fail("This quantity is not available!");
            }

            if (await iCartRepository.AnyAsync(x => x.PersonFin == personFin && x.ProductId == product.Id && x.Quantity==request.Quantity, cancellationToken))
            {
                return Result.Fail("This Product already exist in cart!");
            }

            if (await iCartRepository.AnyAsync(x=>x.PersonFin==personFin && x.ProductId==product.Id && x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.NotOrdered),cancellationToken))
            {
                var xCart = await iCartRepository.FirstOrDefaultAsync(
                    x => x.PersonFin == personFin && x.ProductId == product.Id && x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.NotOrdered), cancellationToken);
                xCart.Quantity = request.Quantity;
                xCart.Price = request.Quantity * product.Price;
                 iCartRepository.Update(xCart);
                 await iUnitOfWork.SaveChangesAsync(cancellationToken);
                 return Result.Ok();

            }
            var cart = request.Adapt<CartEntity>();
            cart.ProductId = product.Id;
            cart.PersonFin = personFin;
            cart.OrderStatus = nameof(CreateOrderCommand.OrderStatus.NotOrdered);
            cart.Price = product.Price *request.Quantity ;
            cart.IsSelected = true;
            await iCartRepository.AddAsync(cart,cancellationToken);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
            
        }
    }
}