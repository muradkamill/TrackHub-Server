using System.Globalization;
using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.Category;

public class GetByNameCategoryQuery
{
    public class GetByIdCategoryRequest():IRequest<Result<GetByIdCategoryResponse>>
    {
        public string Name { get; set; } = default!;
    }

    public class GetByIdCategoryResponse()
    {
        public int CategoryId { get; set; } = default!;
        public string CategoryName { get; set; } = default!;

    }
    public class GetByIdCategoryRequestHandler(ICategoryRepository iCategoryRepository):IRequestHandler<GetByIdCategoryRequest,Result<GetByIdCategoryResponse>>
    {
        public async Task<Result<GetByIdCategoryResponse>> Handle(GetByIdCategoryRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.Name = textInfo.ToTitleCase(request.Name.ToLowerInvariant());
            var isAnyCategory = await iCategoryRepository.AnyAsync(x=>x.CategoryName==request.Name, cancellationToken);
            if (!isAnyCategory)
            {
                return Result.Fail("Category is not found");
            }

            var category =await iCategoryRepository.FirstOrDefaultAsync(x=>x.CategoryName==request.Name, cancellationToken);
            var newCategory=category.Adapt<GetByIdCategoryResponse>();

            return Result.Ok(newCategory);

        }
    }
}