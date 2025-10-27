using System.Globalization;
using Application.Interfaces;
using Domain.Category;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.Category.CRUD;

public class CreateCategory
{
    public class CreateCategoryRequest():IRequest<Result>
    {
        public string CategoryName { get; set; } = default!;  
    }
    public class CreateCategoryRequestHandler(ICategoryRepository iCategoryRepository,IUnitOfWork iUnitOfWork):IRequestHandler<CreateCategoryRequest,Result>
    {
        public async Task<Result> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.CategoryName = textInfo.ToTitleCase(request.CategoryName.ToLowerInvariant());
            
            if (await iCategoryRepository.AnyAsync(x=>x.CategoryName==request.CategoryName,cancellationToken))
            {
                return Result.Fail("Category name already exist");
            }

            var newCategory = request.Adapt<CategoryEntity>();
            await iCategoryRepository.AddAsync(newCategory, cancellationToken);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}