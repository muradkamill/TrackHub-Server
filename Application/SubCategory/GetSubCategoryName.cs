using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.SubCategory;

public class GetSubCategoryName
{
    public class GetSubCategoryNameRequest:IRequest<Result<string>>
    {
        public int SubCategoryId { get; set; }

    }
    public class GetCategoryNameRequestHandler(ISubCategoryRepository iSubCategoryRepository):IRequestHandler<GetSubCategoryNameRequest,Result<string>>
    {
        public async Task<Result<string>> Handle(GetSubCategoryNameRequest request, CancellationToken cancellationToken)
        {
            var subCategory =await iSubCategoryRepository.FirstOrDefaultAsync(x=>x.CategoryId==request.SubCategoryId, cancellationToken);
            return Result.Ok(subCategory.SubCategoryName);

        }
    }

}