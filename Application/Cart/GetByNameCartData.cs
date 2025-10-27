using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Cart;

public class GetByNameCartData
{
    public class GetAllCartDataRequest() : IRequest<Result<IQueryable<GetByNameCartDataResponse>>>
    {

    }

    public class GetByNameCartDataResponse()
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ProductOwnerFin { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> ImageUrls { get; set; } = default!;
        public bool IsSelected { get; set; }
        public int StockQuantity { get; set; }


    }
    public class GetByNameCartDataRequestHandler(ICartRepository iCartRepository,IProductRepository iProductRepository,IHttpContextAccessor httpContextAccessor):IRequestHandler<GetAllCartDataRequest,Result<IQueryable<GetByNameCartDataResponse>>>
    {
        public async Task<Result<IQueryable<GetByNameCartDataResponse>>> Handle(GetAllCartDataRequest request, CancellationToken cancellationToken)
        {
            var response = new List<GetByNameCartDataResponse>();
            var personFin = httpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access!");


            if (!await iCartRepository.AnyAsync(x => x.PersonFin == personFin, cancellationToken))
            {
                return Result.Fail("Cart is not found!");
            }

            var carts= iCartRepository.Where(x => x.PersonFin == personFin && x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.NotOrdered)).ToList();
            foreach (var cart in carts)
            {
                var product =await iProductRepository.FirstOrDefaultAsync(x=>x.Id==cart.ProductId,cancellationToken);
                response.Add(new GetByNameCartDataResponse()
                {
                    Description = product.Description,
                    Id = cart.Id,
                    Price = cart.Price,
                    ProductName = product.Name,
                    Quantity = cart.Quantity,
                    ProductOwnerFin=product.OwnerFin,
                    ImageUrls=product.ImageUrls,
                    IsSelected = cart.IsSelected,
                    StockQuantity = product.StockQuantity,
                });

            }

            return Result.Ok(response.AsQueryable());

        }
    }
}