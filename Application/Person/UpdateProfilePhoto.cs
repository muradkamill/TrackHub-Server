using System.Globalization;
using Application.Interfaces;
using Domain;
using Domain.Auth;
using FluentResults;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace Application.Person;

public class UpdateProfilePhoto()
{
    public class UpdateProfilePhotoRequest():IRequest<Result>
    {
        public IFormFile Image { get; set; } = default!;

    }
    
    
    public class UpdateProfilePhotoRequestHandler(IPersonRepository iPersonRepository,IUnitOfWork iUnitOfWork,IWebHostEnvironment env,IHttpContextAccessor httpContextAccessor):IRequestHandler<UpdateProfilePhotoRequest,Result>
    {
        public async Task<Result> Handle(UpdateProfilePhotoRequest request, CancellationToken cancellationToken)
        {

            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");
            if (request.Image.Length==0)
            {
                return Result.Fail("Image is not valid");
            }

            var extensions = Path.GetExtension (request.Image.FileName).ToLowerInvariant();
            var allowedExtensions=new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowedExtensions.Contains(extensions))
            {
                return Result.Fail("Image format  is not valid");

            }

            var uploadsFolder = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream,cancellationToken);
            };
            
            var relativePath = "/uploads/" + fileName;
            
            
            var person =await iPersonRepository.FirstOrDefaultAsync(x => x.Fin == personFin,cancellationToken);
            if (person is null)
            {
                return Result.Fail("Person is not found!");

            }
            var newPerson=request.Adapt(person);
            newPerson.ImageUrl=relativePath;
            iPersonRepository.Update(newPerson);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();

        }
    }
}