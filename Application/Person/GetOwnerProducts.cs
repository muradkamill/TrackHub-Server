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
        public List<string> ImageUrls { get; set; } = default!;
    }
    public class GetOwnerProductsRequestHandler(IProductRepository iProductRepository,IHttpContextAccessor httpContextAccessor):IRequestHandler<GetOwnerProductsRequest,Result<IQueryable<GetOwnerProductsResponse>>>
    {
        public async Task<Result<IQueryable<GetOwnerProductsResponse>>> Handle(GetOwnerProductsRequest request, CancellationToken cancellationToken)
        {
            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");
            var products = iProductRepository.Where(x => x.OwnerFin == personFin);
            var response = products.ProjectToType<GetOwnerProductsResponse>();
            return await Task.FromResult(Result.Ok(response));
        }
    }
}