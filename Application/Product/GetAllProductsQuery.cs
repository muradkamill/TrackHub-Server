using Application.Interfaces;
using Application.SubCategory;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Product;

public class GetAllProductsQuery
{

    public class GetAllQueryRequest() : IRequest<Result<IQueryable<GetAllQueryResponse>>>
    {
    }

    public class GetAllQueryResponse()
    {
        public int Id { get; set; }
        public string OwnerFin { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public decimal ProductRate { get; set; }

        public List<string> ImageUrls { get; set; } = default!;
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = default!;
    }

    public class GetAllProductsHandler(
        IProductRepository iProductRepository,
        ISubCategoryRepository iSubCategoryRepository)
        : IRequestHandler<GetAllQueryRequest, Result<IQueryable<GetAllQueryResponse>>>
    {
        public async Task<Result<IQueryable<GetAllQueryResponse>>> Handle(GetAllQueryRequest request,
            CancellationToken cancellationToken)
        {
            var allProducts = iProductRepository.GetAll();


            var newAllProducts = allProducts.ProjectToType<GetAllQueryResponse>().ToList();
            foreach (var product in newAllProducts)
            {
                var subCategory = await iSubCategoryRepository.FirstOrDefaultAsync(x => x.Id == product.SubCategoryId,cancellationToken);
                product.SubCategoryName = subCategory.SubCategoryName;
            }

            return Result.Ok(newAllProducts.AsQueryable());
        }
    }
}