using Application.Cart;
using Application.Interfaces;
using Application.Product;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Person;

public class GetOwnerProducts
{
    public class GetOwnerProductsRequest : IRequest<Result<IQueryable<GetOwnerProductsResponse>>>
    {
    }

    public class GetOwnerProductsResponse()
    {
        public int Id { get; set; }
        public string OwnerFin { get; set; } = default!;
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal ProductRate { get; set; }
        public int StockQuantity { get; set; }
        public string ApplicationStatus { get; set; } = default!;
        public string SubCategoryName { get; set; } = default!;

        public List<string> ImageUrls { get; set; } = default!;
    }
    public class GetOwnerProductsRequestHandler(IProductRepository iProductRepository,IHttpContextAccessor httpContextAccessor,ICartRepository iCartRepository,ISubCategoryRepository iSubCategoryRepository):IRequestHandler<GetOwnerProductsRequest,Result<IQueryable<GetOwnerProductsResponse>>>
    {
        public async Task<Result<IQueryable<GetOwnerProductsResponse>>> Handle(GetOwnerProductsRequest request, CancellationToken cancellationToken)
        {
            var list = new List<GetOwnerProductsResponse>();
            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");
            var products = iProductRepository.Where(x => x.OwnerFin == personFin).ToList();

            foreach (var product in products)
            {
                var subCategory =await iSubCategoryRepository.FirstOrDefaultAsync(x => x.Id == product.SubCategoryId,cancellationToken);
                var response = product.Adapt<GetOwnerProductsResponse>();
                response.SubCategoryName = subCategory.SubCategoryName;

                var carts = iCartRepository.Where(x => x.ProductId == product.Id);
                if (carts.Any(x=>x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.Delivered)))
                {
                    response.ApplicationStatus = nameof(CreateOrderCommand.OrderStatus.Delivered);

                }
                else if (carts.Any(x=>x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.NotOrdered) || x.OrderStatus=="NULL"))
                {
                    response.ApplicationStatus = nameof(CreateOrderCommand.OrderStatus.NotOrdered);
                }
                else if (carts.Any(x=>x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.Cancelled)))
                {
                    response.ApplicationStatus = nameof(CreateOrderCommand.OrderStatus.Cancelled);
                }
                else if (carts.Any(x=>x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.CourierAccept)) || carts.Any(x=>x.OrderStatus==nameof(CreateOrderCommand.OrderStatus.CourierPending)))
                {
                    response.ApplicationStatus = "Ordered";
                }
                else
                {
                    response.ApplicationStatus = nameof(CreateOrderCommand.OrderStatus.NotOrdered);
                }
                list.Add(response);


            }
            return await Task.FromResult(Result.Ok(list.AsQueryable()));
        }
    }
}