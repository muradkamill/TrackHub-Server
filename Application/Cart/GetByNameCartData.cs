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
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ProductOwnerFin { get; set; } = default!;
        public string ProductOwnerName { get; set; } = default!;
        public string ProductOwnerSurName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> ImageUrls { get; set; } = default!;
        public bool IsSelected { get; set; }
        public int StockQuantity { get; set; }


    }
    public class GetByNameCartDataRequestHandler(ICartRepository iCartRepository,IProductRepository iProductRepository,IHttpContextAccessor httpContextAccessor,IPersonRepository iPersonRepository):IRequestHandler<GetAllCartDataRequest,Result<IQueryable<GetByNameCartDataResponse>>>
    {
        public async Task<Result<IQueryable<GetByNameCartDataResponse>>> Handle(GetAllCartDataRequest request, CancellationToken cancellationToken)
        {
            var personFin = httpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access!");
            // var personFin = "820RD60";

            var response = new List<GetByNameCartDataResponse>();


            var carts= iCartRepository.Where(x => x.PersonFin == personFin && x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.NotOrdered)).ToList();
            foreach (var cart in carts)
            {
                var product =await iProductRepository.FirstOrDefaultAsync(x=>x.Id==cart.ProductId,cancellationToken);
                if (product == null)
                {
                    continue;
                }

                var owner =await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==product.OwnerFin, cancellationToken);
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
                    ProductId = product.Id,
                    ProductOwnerName =owner.Name,
                    ProductOwnerSurName = owner.SurName,
                });

            }

            return Result.Ok(response.AsQueryable());

        }
    }
}