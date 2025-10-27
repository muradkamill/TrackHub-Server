using System.Globalization;
using Application.Category;
using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.SubCategory;

public class UpdateByNameSubCategory
{
    public class UpdateByNameSubCategoryRequest() : IRequest<Result>
    {
        public string SubCategoryName { get; set; } = default!;
        public string NewSubCategoryName { get; set; } = default!;
        public string CategoryName { get; set; } = default!;



        
    }
    public class UpdateByNameSubCategoryRequestHandler(ISubCategoryRepository iSubCategoryRepository,ICategoryRepository iCategoryRepository,IUnitOfWork iUnitOfWork):IRequestHandler<UpdateByNameSubCategoryRequest,Result>
    {
        public async Task<Result> Handle(UpdateByNameSubCategoryRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.SubCategoryName = textInfo.ToTitleCase(request.SubCategoryName.ToLowerInvariant());
            request.NewSubCategoryName = textInfo.ToTitleCase(request.NewSubCategoryName.ToLowerInvariant());
            if (!await iSubCategoryRepository.AnyAsync(x=>x.SubCategoryName==request.SubCategoryName,cancellationToken))
            {
                return Result.Fail("SubCategory is not found!");
            }
            if (!await iCategoryRepository.AnyAsync(x=>x.CategoryName==request.CategoryName,cancellationToken))
            {
                return Result.Fail("Category is not found!");
            }
            var subCategory = await iSubCategoryRepository.FirstOrDefaultAsync(x => x.SubCategoryName == request.SubCategoryName, cancellationToken);
                subCategory.SubCategoryName = request.NewSubCategoryName;
            var category =await iCategoryRepository.FirstOrDefaultAsync(x => x.CategoryName == request.CategoryName, cancellationToken);
            subCategory.CategoryId = category.CategoryId;
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}