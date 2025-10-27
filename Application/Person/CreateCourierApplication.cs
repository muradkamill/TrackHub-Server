using System.Globalization;
using Application.Auth;
using Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using IUnitOfWork = Application.Interfaces.IUnitOfWork;

namespace Application.Person;

public class CreateCourierApplication
{
    public enum VehicleTypeEnum
    {
        None,
        Bicycle,
        Motorcycle,
        Car,
        Van,
        Truck
    }

    public enum ApplicationStatusEnum
    {
        Pending,
        Applied,
        Rejected
    }

    public class CreateApplicationCourierRequest : IRequest<Result>
    {
        public IFormFile CvUrl { get; set; } = default!;
        public string VehicleType { get; set; } = default!;
    }
    public class CreateApplicationCourierRequestHandler(IPersonRepository iPersonRepository,IUnitOfWork iUnitOfWork,IWebHostEnvironment env):IRequestHandler<CreateApplicationCourierRequest,Result>
    {
        
        public async Task<Result> Handle(CreateApplicationCourierRequest request, CancellationToken cancellationToken)
        {
            // var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            // if (string.IsNullOrWhiteSpace(personFin))
            // {
            //     return Result.Fail("Unauthorized access !");
            // }
            var personFin = "2222222";

            var person = await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==personFin, cancellationToken);


            if (person.Role==nameof(RegisterCommand.RoleEnum.Courier) )
            {
                return Result.Fail("You are already courier!");
            }
            
            var textInfo= CultureInfo.InvariantCulture.TextInfo;
            request.VehicleType = textInfo.ToTitleCase(request.VehicleType.ToLowerInvariant());
            

            if (request.CvUrl.Length==0)
            {
                return Result.Fail("Image is not valid");
            }

            var extensions = Path.GetExtension (request.CvUrl.FileName).ToLowerInvariant();
            var allowedExtensions=new[] { ".pdf", ".doc", ".docx", ".odt", ".rtf", ".txt" };
            if (!allowedExtensions.Contains(extensions))
            {
                return Result.Fail("Image format  is not valid");

            }

            var uploadsFolder = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(request.CvUrl.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.CvUrl.CopyToAsync(stream,cancellationToken);
            };
            
            var relativePath = "/uploads/" + fileName;

            if (!Enum.IsDefined(typeof(VehicleTypeEnum),request.VehicleType))
            {
                return Result.Fail("Vehicle type is not defined !");
            }

            person.VehicleType = request.VehicleType;
            person.CvUrl = relativePath;
            person.ApplicationStatus = nameof(ApplicationStatusEnum.Pending);
            iPersonRepository.Update(person);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();


        }
    }
}