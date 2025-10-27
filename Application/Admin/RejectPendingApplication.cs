using System.Globalization;
using Application.Interfaces;
using Application.Person;
using FluentResults;
using MediatR;

namespace Application.Admin;

public class RejectPendingApplication
{
    public class RejectPendingApplicationRequest:IRequest<Result>
    {
        public string PersonFin { get; set; } = default!;
    }
    public class RejectPendingApplicationRequestHandler(IPersonRepository iPersonRepository,IUnitOfWork iUnitOfWork):IRequestHandler<RejectPendingApplicationRequest,Result>
    {
        public async Task<Result> Handle(RejectPendingApplicationRequest request, CancellationToken cancellationToken)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            request.PersonFin = textInfo.ToUpper(request.PersonFin);
            var person = await iPersonRepository.FirstOrDefaultAsync(x=>x.ApplicationStatus=="Pending" && x.Fin==request.PersonFin, cancellationToken);
            if (person is null)
            {
                return Result.Fail("Application is not found!");
            }

            person.ApplicationStatus =nameof(CreateCourierApplication.ApplicationStatusEnum.Rejected);

            iPersonRepository.Update(person);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();        }
    }
}