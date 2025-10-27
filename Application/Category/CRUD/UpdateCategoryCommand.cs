using System.Globalization;
using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Category.CRUD;

public class UpdateCategoryCommand
{
    public class UpdateCategoryRequest():IRequest<Result>
    {
        public string CategoryName { get; set; } = default!;
        public string NewCategoryName { get; set; } = default!;


    }

    public class UpdateCategoryCommandHandler(ICategoryRepository iCategoryRepository,IUnitOfWork iUnitOfWork):IRequestHandler<UpdateCategoryRequest,Result>
    {
        public async Task<Result> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.CategoryName = textInfo.ToTitleCase(request.CategoryName.ToLowerInvariant());

                request.NewCategoryName = textInfo.ToTitleCase(request.NewCategoryName.ToLowerInvariant());

                
            if (!await iCategoryRepository.AnyAsync(x=>x.CategoryName==request.CategoryName,cancellationToken))
            {
                return Result.Fail("Category is not exist!");
            }
            if (await iCategoryRepository.AnyAsync(x=>x.CategoryName==request.NewCategoryName,cancellationToken))
            {
                return Result.Fail("New Category name already used!");

            }
            var category = await iCategoryRepository.FirstOrDefaultAsync(x=>x.CategoryName==request.CategoryName,cancellationToken);
            if (request.NewCategoryName != null)
            {
                category.CategoryName = request.NewCategoryName;

            }
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}