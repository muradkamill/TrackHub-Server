using System.Globalization;
using Application.Category;
using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.SubCategory;

public class GetByNameSubCategory
{
    public class GetByNameSubCategoryRequest():IRequest<Result<GetByNameSubCategoryResponse>>
    {
        public string SubCategoryName { get; set; } = default!;
    }

    public class GetByNameSubCategoryResponse()
    {
        public string SubCategoryName { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
    }
    public class GetByNameSubCategoryRequestHandler(ISubCategoryRepository iSubCategoryRepository,ICategoryRepository iCategoryRepository):IRequestHandler<GetByNameSubCategoryRequest,Result<GetByNameSubCategoryResponse>>
    {
        public async Task<Result<GetByNameSubCategoryResponse>> Handle(GetByNameSubCategoryRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.SubCategoryName = textInfo.ToTitleCase(request.SubCategoryName.ToLowerInvariant());
            if (!await iSubCategoryRepository.AnyAsync(x=>x.SubCategoryName==request.SubCategoryName,cancellationToken))
            {
                return Result.Fail("SubCategory is not found!");
            }

            var subcategory = await iSubCategoryRepository.FirstOrDefaultAsync(x => x.SubCategoryName == request.SubCategoryName, cancellationToken);
            var newSubCategory = subcategory.Adapt<GetByNameSubCategoryResponse>();

            var category =
                await iCategoryRepository.FirstOrDefaultAsync(x => x.CategoryId == subcategory.CategoryId,
                    cancellationToken);

            newSubCategory.CategoryName = category.CategoryName;
            return Result.Ok(new GetByNameSubCategoryResponse()
            {
                CategoryName =newSubCategory.CategoryName ,
                SubCategoryName =newSubCategory.SubCategoryName,
            });


        }
    }
}