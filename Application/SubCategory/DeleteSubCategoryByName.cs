using System.Globalization;
using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.SubCategory;

public class DeleteSubCategoryByName
{
    public class DeleteSubCategoryByNameRequest():IRequest<Result>
    {
        public string SubCategoryName { get; set; } = default!;

    }
    public class DeleteSubCategoryByNameRequestHandler(ISubCategoryRepository iSubCategoryRepository,IUnitOfWork iUnitOfWork):IRequestHandler<DeleteSubCategoryByNameRequest,Result>
    {
        public async Task<Result> Handle(DeleteSubCategoryByNameRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.SubCategoryName = textInfo.ToTitleCase(request.SubCategoryName.ToLowerInvariant());
            if (!await iSubCategoryRepository.AnyAsync(x=>x.SubCategoryName==request.SubCategoryName,cancellationToken))
            {
                return Result.Fail("SubCategory is not exist!");
            }

            var subCategory = await iSubCategoryRepository.FirstOrDefaultAsync(x => x.SubCategoryName == request.SubCategoryName, cancellationToken);
            subCategory.IsDelete = true;
            subCategory.DeleteAt=DateTime.UtcNow;
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();



        }
    }
}