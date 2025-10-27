using System.Globalization;
using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Category.CRUD;

public class DeleteByNameCommand
{
    public class DeleteByNameCategoryRequest : IRequest<Result>
    {
        public string CategoryName { get; set; } = default!;
    }

    public class DeleteByIdCommandRequestHandler(ICategoryRepository iCategoryRepository,IUnitOfWork iUnitOfWork):IRequestHandler<DeleteByNameCategoryRequest,Result>
    {
        public async Task<Result> Handle(DeleteByNameCategoryRequest request, CancellationToken cancellationToken)
        {
            
            if (request.CategoryName==null)
            {
                return Result.Fail("Category name cannot be null!");
            }
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.CategoryName = textInfo.ToTitleCase(request.CategoryName.ToLowerInvariant());
            
            if (!await iCategoryRepository.AnyAsync(x=>x.CategoryName==request.CategoryName,cancellationToken))
            {
                return Result.Fail("Category is not found!");
            }

            var category =await  iCategoryRepository.FirstOrDefaultAsync(x=>x.CategoryName==request.CategoryName,cancellationToken);
            if (category is null)
            {
                return Result.Fail("Category is not found!");
            }
            category.IsDelete = true;
            category.DeleteAt=DateTime.UtcNow;
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}