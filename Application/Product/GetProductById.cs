using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.Product;

public class GetProductById
{
    public class GetProductByIdRequest : IRequest<Result<GetProductByIdResponse>>
    {
        public int ProductId { get; set; }
    }

    public class GetProductByIdResponse
    {
        public string OwnerFin { get; set; } = default!;
        public string OwnerName { get; set; } = default!;
        public string OwnerSurname { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public bool IsNew { get; set; }
        public decimal ProductRate { get; set; }
        public int RateQuantity { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StockQuantity { get; set; }
        public List<string> ImageUrls { get; set; } = default!;
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = default!;

    }
    public class GetProductByIdRequestHandler(IProductRepository iProductRepository,ISubCategoryRepository iSubCategoryRepository,IPersonRepository iPersonRepository):IRequestHandler<GetProductByIdRequest,Result<GetProductByIdResponse>>
    {
        public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            if (!await iProductRepository.AnyAsync(x=>x.Id==request.ProductId,cancellationToken))
            {
                return Result.Fail("Product is not found!");
            }

            var product = await iProductRepository.FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);
            var owner =await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==product.OwnerFin, cancellationToken);
            var subCategory = await iSubCategoryRepository.FirstOrDefaultAsync(x=>x.Id==product.SubCategoryId,cancellationToken);
            var response = product.Adapt<GetProductByIdResponse>();
            response.SubCategoryName = subCategory.SubCategoryName;
            response.OwnerName = owner.Name;
            response.OwnerSurname = owner.SurName;

            return Result.Ok(response);
        }
    }
}