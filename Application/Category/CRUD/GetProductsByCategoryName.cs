using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.Category.CRUD;

public class GetProductsByCategoryName
{
    public class GetProductsByCategoryNameResponse()
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


    public class GetProductsByCategoryNameRequest : IRequest<Result<IQueryable<GetProductsByCategoryNameResponse>>>
    {
        public int CategoryId { get; set; }
    }

    public class GetProductsByCategoryNameRequestHandler(IProductRepository iProductRepository,ISubCategoryRepository iSubCategoryRepository,ICategoryRepository iCategoryRepository) : IRequestHandler<GetProductsByCategoryNameRequest,Result<IQueryable<GetProductsByCategoryNameResponse>>>
    {
        public async Task<Result<IQueryable<GetProductsByCategoryNameResponse>>> Handle(GetProductsByCategoryNameRequest request, CancellationToken cancellationToken)
        {
            var response =new List<GetProductsByCategoryNameResponse>();
            if (!await iCategoryRepository.AnyAsync(x=>x.CategoryId==request.CategoryId,cancellationToken))
            {
                return Result.Fail("Category is not found!");
            }

            var category = await iCategoryRepository.FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId, cancellationToken);
            var subCategories =iSubCategoryRepository.Where(x => x.CategoryId == category.CategoryId).ToList();
            foreach (var subCategory in subCategories)
            {
                var products = iProductRepository.Where(x => x.SubCategoryId == subCategory.Id).ToList();
                var x = products.Adapt<List<GetProductsByCategoryNameResponse>>();
                response.AddRange(x);
            }
            return Result.Ok(response.AsQueryable());
        }
    }



}