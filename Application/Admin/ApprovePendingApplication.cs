using System.Globalization;
using Application.Auth;
using Application.Interfaces;
using Application.Person;
using FluentResults;
using MediatR;

namespace Application.Admin;

public class 
    ApprovePendingApplication
{
    public class ApprovePendingApplicationRequest:IRequest<Result>
    {
        public string PersonFin { get; set; } = default!;
    }
    public class ApprovePendingApplicationRequestHandler(IPersonRepository iPersonRepository,IUnitOfWork iUnitOfWork):IRequestHandler<ApprovePendingApplicationRequest,Result>
    {
        public async Task<Result> Handle(ApprovePendingApplicationRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.PersonFin = textInfo.ToUpper(request.PersonFin);
            var person = await iPersonRepository.FirstOrDefaultAsync(x=>x.ApplicationStatus=="Pending" && x.Fin==request.PersonFin, cancellationToken);
            if (person is null)
            {
                return Result.Fail("Application is not found!");
            }

            person.ApplicationStatus =nameof(CreateCourierApplication.ApplicationStatusEnum.Applied);
            person.Role = nameof(RegisterCommand.RoleEnum.Courier);
            iPersonRepository.Update(person);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}