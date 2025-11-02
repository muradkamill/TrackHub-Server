using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Product;

public class GetProductByName
{
    public class GetProductByNameRequest:IRequest<Result<IQueryable<GetProductByNameResponse>>>
    {
        public string ProductName { get; set; } = default!;

    }
    public class GetProductByNameResponse
    {
        public string SubCategoryName { get; set; } = default!;
        public List<string> ImageUrls { get; set; } = default!;
        public decimal ProductRate { get; set; }
        public string Description { get; set; } = default!;
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; } = default!;
    }
    public class GetProductByNameRequestHandler(IProductRepository iProductRepository,ISubCategoryRepository iSubCategoryRepository):IRequestHandler<GetProductByNameRequest,Result<IQueryable<GetProductByNameResponse>>>
    {
        public async Task<Result<IQueryable<GetProductByNameResponse>>> Handle(GetProductByNameRequest request, CancellationToken cancellationToken)
        {
            var list = new List<GetProductByNameResponse>();
            var products = await iProductRepository.Where(x => x.Name == request.ProductName)
                .ToListAsync(cancellationToken);
            foreach (var product in products)
            {
                var subCategory =await iSubCategoryRepository.FirstOrDefaultAsync(x=>x.Id==product.SubCategoryId, cancellationToken);
                var res = product.Adapt<GetProductByNameResponse>();
                res.SubCategoryName = subCategory.SubCategoryName;
                list.Add(res);
            }
            return Result.Ok(list.AsQueryable());


        }
    }

}