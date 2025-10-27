using System.Globalization;
using Application.Category;
using Application.Interfaces;
using Domain.Product;
using Domain.SubCategory;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.SubCategory;

public class CreateSubCategoryCommand
{
    public class CreateSubCategoryRequest() : IRequest<Result>
    {
        public string SubCategoryName { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
    }
    public class CreateSubCategoryRequestHandler(ISubCategoryRepository iSubCategoryRepository,ICategoryRepository iCategoryRepository,IUnitOfWork iUnitOfWork):IRequestHandler<CreateSubCategoryRequest,Result>
    {
        public async Task<Result> Handle(CreateSubCategoryRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            if (await iSubCategoryRepository.AnyAsync(x=>x.SubCategoryName==request.SubCategoryName,cancellationToken))
            {
                return Result.Fail("SubCategory already exist!");

            }

            request.CategoryName = textInfo.ToTitleCase(request.CategoryName.ToLowerInvariant());
            request.SubCategoryName = textInfo.ToTitleCase(request.SubCategoryName.ToLowerInvariant());

            

            var newSubCategory = request.Adapt<SubCategoryEntity>();
            var category = await iCategoryRepository.FirstOrDefaultAsync(x=>x.CategoryName==request.CategoryName, cancellationToken);

            if (category is null)
            {
                return Result.Fail("Category is not found!");
            }

            
            newSubCategory.CategoryId=category.CategoryId;

            await iSubCategoryRepository.AddAsync(newSubCategory, cancellationToken);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();


        }
    }
}