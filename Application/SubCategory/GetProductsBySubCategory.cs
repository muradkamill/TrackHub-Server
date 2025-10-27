using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.SubCategory;

public class GetProductsBySubCategory
{
    public class GetProductsBySubCategoryResponse()
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
    public class GetProductsBySubCategoryRequest : IRequest<Result<IQueryable<GetProductsBySubCategoryResponse>>>
    {
        public int SubCategoryId { get; set; }

    }
    public class GetProductsBySubCategoryRequestHandler(IProductRepository iProductRepository,ISubCategoryRepository iSubCategoryRepository):IRequestHandler<GetProductsBySubCategoryRequest,Result<IQueryable<GetProductsBySubCategoryResponse>>>
    {
        public async Task<Result<IQueryable<GetProductsBySubCategoryResponse>>> Handle(GetProductsBySubCategoryRequest request, CancellationToken cancellationToken)
        {
            if (!await iProductRepository.AnyAsync(x=>x.SubCategoryId==request.SubCategoryId,cancellationToken))
            {
                return Result.Fail("Product is not Found!");
            }
            var products =iProductRepository.Where(x => x.SubCategoryId == request.SubCategoryId).ToList();
            var response = products.Adapt<List<GetProductsBySubCategoryResponse>>();
            foreach (var product in response)
            {
                var subCategory = await iSubCategoryRepository.FirstOrDefaultAsync(x => x.Id == product.SubCategoryId,cancellationToken);
                product.SubCategoryName = subCategory.SubCategoryName;
            }
            return Result.Ok(response.AsQueryable());
        }
    }
}