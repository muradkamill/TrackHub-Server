using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.SubCategory;

public class GetAllSubCategoryQuery
{
    public class GetAllSubCategoryRequest():IRequest<Result<IQueryable<GetAllSubCategoryResponse>>>
    {
        
    }
    public class GetAllSubCategoryResponse()
    {
        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; } = default!;
        public string CategoryName { get; set; } = default!;

    }
    public class GetAllSubCategoryRequestHandler(ISubCategoryRepository iSubCategoryRepository,ICategoryRepository iCategoryRepository):IRequestHandler<GetAllSubCategoryRequest,Result<IQueryable<GetAllSubCategoryResponse>>>
    {
        public async Task<Result<IQueryable<GetAllSubCategoryResponse>>> Handle(GetAllSubCategoryRequest request, CancellationToken cancellationToken)
        {
            var list = new List<GetAllSubCategoryResponse>();
            var subCategories=iSubCategoryRepository.GetAll().ToList();
            foreach (var subCategory in subCategories)
            {
                var category =await iCategoryRepository.FirstOrDefaultAsync(x=>x.CategoryId==subCategory.CategoryId,cancellationToken);
                list.Add(new GetAllSubCategoryResponse()
                {
                    CategoryName =category.CategoryName ,
                    SubCategoryName = subCategory.SubCategoryName,
                    SubCategoryId = subCategory.Id
                });

            }
            return Result.Ok(list.AsQueryable());


        }
    }
}