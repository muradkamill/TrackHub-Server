using System.Globalization;
using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Product;

public class DeleteProductByName
{
    public class DeleteProductByNameRequest() : IRequest<Result>
    {
        public string ProductName { get; set; } = default!;
    }
    
    public class DeleteProductByNameRequestHandler(IProductRepository iProductRepository,IUnitOfWork iUnitOfWork):IRequestHandler<DeleteProductByNameRequest,Result>
    {
        public async Task<Result> Handle(DeleteProductByNameRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.ProductName = textInfo.ToTitleCase(request.ProductName.ToLowerInvariant());

            if (!await iProductRepository.AnyAsync(x=>x.Name==request.ProductName,cancellationToken) )
            {
                return Result.Fail("Product is not found!");
            }

            var product = await iProductRepository.FirstOrDefaultAsync(x=>x.Name==request.ProductName,cancellationToken);
            product.IsDelete = true;
            product.DeleteAt=DateTime.UtcNow;
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}