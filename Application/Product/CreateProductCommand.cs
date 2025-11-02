using System.Globalization;
using Application.Interfaces;
using Application.SubCategory;
using Domain.Product;
using FluentResults;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Product;

public class CreateProductCommand
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(2).WithMessage("Name must be minimum 2 character")
                .MaximumLength(30).WithMessage("Name must be maximum 30 character");
            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Description must be maximum 200 character");
            RuleFor(x => x.Weight).InclusiveBetween(0, 500);
            // RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");
            // RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");
            RuleFor(x => x.Price).InclusiveBetween(0,100000).WithMessage("Product price must be between 0 and 100000");
            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("At least one image is required")
                .Must(images => images.All(i => i.Length > 0))
                .WithMessage("All images must be valid files");


        
        }
    }
    public class CreateProductRequest():IRequest<Result>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsNew { get; set; }
        public decimal Weight { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<IFormFile> Images { get; set; } = new();
        public string SubCategoryName { get; set; } = default!;
    }

    
    public class CreateProductRequestHandler(IProductRepository iProductRepository,ISubCategoryRepository iSubCategoryRepository,IUnitOfWork iUnitOfWork,IWebHostEnvironment env,IHttpContextAccessor httpContextAccessor):IRequestHandler<CreateProductRequest,Result>
    {
        public async Task<Result> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var  imagePaths=new List<string>();
            // var personFin = "820RD60";
            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");
            if (!request.Images.Any())
            {
                return Result.Fail("Image is not valid");
            }

            var uploadsFolder = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var allowedExtensions=new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            foreach (var image in request.Images)
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
            
            var newProducts = request.Adapt<ProductEntity>();
            if (!await iSubCategoryRepository.AnyAsync(x => x.SubCategoryName == request.SubCategoryName,cancellationToken)) 
            {
                return Result.Fail("SubCategory is not found!");

            }
            var subCategory = await iSubCategoryRepository.FirstOrDefaultAsync(x => x.SubCategoryName == request.SubCategoryName, cancellationToken);
            newProducts.SubCategoryId = subCategory.Id;
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            newProducts.Name = textInfo.ToTitleCase(newProducts.Name.ToLowerInvariant());
            newProducts.Description = textInfo.ToTitleCase(newProducts.Description.ToLowerInvariant());
            newProducts.ImageUrls=imagePaths;
            newProducts.OwnerFin = personFin;
            newProducts.ProductRate = 0;
            
            
            
            await iProductRepository.AddAsync(newProducts, cancellationToken);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}