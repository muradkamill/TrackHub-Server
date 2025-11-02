using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Category.CRUD;

public class GetCategoryName
{
    public class GetCategoryNameRequest:IRequest<Result<string>>
    {
        public int CategoryId { get; set; }

    }
    public class GetCategoryNameRequestHandler(ICategoryRepository iCategoryRepository):IRequestHandler<GetCategoryNameRequest,Result<string>>
    {
        public async Task<Result<string>> Handle(GetCategoryNameRequest request, CancellationToken cancellationToken)
        {
            var category =await iCategoryRepository.FirstOrDefaultAsync(x=>x.CategoryId==request.CategoryId, cancellationToken);
            return Result.Ok(category.CategoryName);

        }
    }

}