using System.Globalization;
using Application.Interfaces;
using Application.SubCategory;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Application.Product;

public class UpdateProductByName
{
    public class UpdateProductByNameRequest():IRequest<Result>
    {
        public string ProductName { get; set; } = default!;
        public UpdateProductParameters Parameters=default!;

    }

    public class UpdateProductParameters()
    {
        public string NewProductName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        
        public List<IFormFile> Images { get; set; } = default!;
        public string SubCategoryName { get; set; } = default!;
        
    }
    
    public class UpdateProductByNameRequestHandler(IProductRepository iProductRepository,ISubCategoryRepository iSubCategoryRepository,IUnitOfWork iUnitOfWork,IWebHostEnvironment env):IRequestHandler<UpdateProductByNameRequest,Result>
    {
        public async Task<Result> Handle(UpdateProductByNameRequest request, CancellationToken cancellationToken)
        {
            var  imagePaths=new List<string>();

            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.ProductName = textInfo.ToTitleCase(request.ProductName.ToLowerInvariant());


            if (!await iProductRepository.AnyAsync(x=>x.Name==request.ProductName,cancellationToken) )
            {
                return Result.Fail("Product is not found!");
            }
            var product = await iProductRepository.FirstOrDefaultAsync(x=>x.Name==request.ProductName, cancellationToken);


            if (!request.Parameters.Images.Any())
            {
                return Result.Fail("Image is not valid");
            }

            var uploadsFolder = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var allowedExtensions=new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            foreach (var image in request.Parameters.Images)
            {
                var extensions = Path.GetExtension (image.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extensions))
                {
                    return Result.Fail("Image format  is not valid");

                }
                var fileName = Guid.NewGuid() + extensions;
                var filePath = Path.Combine(uploadsFolder, fileName);
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream,cancellationToken);
                };
                var relativePath = "/uploads/" + fileName;
                imagePaths.Add(relativePath);
            }
    
            request.Parameters.SubCategoryName = textInfo.ToTitleCase(request.Parameters.SubCategoryName.ToLowerInvariant());
            request.Parameters.NewProductName = textInfo.ToTitleCase(request.Parameters.NewProductName.ToLowerInvariant());
            request.Parameters.Description = textInfo.ToTitleCase(request.Parameters.Description.ToLowerInvariant());
            if (!await iSubCategoryRepository.AnyAsync(x=>x.SubCategoryName==request.Parameters.SubCategoryName,cancellationToken))
            {
                return Result.Fail("SubCategory is not found!");

            }

            var subCategory = await iSubCategoryRepository.FirstOrDefaultAsync(x=>x.SubCategoryName==request.Parameters.SubCategoryName,cancellationToken);
             

            product.Description = request.Parameters.Description;
            product.Name = request.Parameters.NewProductName;
            product.Price = request.Parameters.Price;
            product.StockQuantity = request.Parameters.StockQuantity;
            product.ImageUrls = imagePaths ;
            product.SubCategoryId = subCategory.Id;





            
            iProductRepository.Update(product);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();


        }
    }
}