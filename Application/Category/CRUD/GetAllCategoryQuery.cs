using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.Category.CRUD;

public class GetAllCategoryQuery
{
    public class GetAllCategoryRequest():IRequest<Result<IQueryable<GetAllCategoryResponse>>>
    {
        
    }

    public class GetAllCategoryResponse()
    {
        public int CategoryId { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
    }
    public class GetAllCategoryRequestHandler(ICategoryRepository iCategoryRepository):IRequestHandler<GetAllCategoryRequest,Result<IQueryable<GetAllCategoryResponse>>>
    {
        public async Task<Result<IQueryable<GetAllCategoryResponse>>> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
        {
            var categories = iCategoryRepository.GetAll();
            var newCategories = categories.ProjectToType<GetAllCategoryResponse>();
            return await Task.FromResult(Result.Ok(newCategories)) ;
        }
    }
}