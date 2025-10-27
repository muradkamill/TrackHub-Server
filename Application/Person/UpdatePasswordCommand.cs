using Application.Interfaces;
using Domain.Auth;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Person;

public class UpdatePasswordCommand
{
    public class UpdatePasswordCommandValidation : AbstractValidator<UpdatePasswordCommandRequest>
    {
        public UpdatePasswordCommandValidation()
        {
            RuleFor(x => x.CurrentPassword).MinimumLength(8)
                .WithMessage("Current Password must be minimum 8 character");
            
            RuleFor(x => x.NewPassword).MinimumLength(8)
                .WithMessage("New Password must be minimum 8 character");
        }
    }
    public class UpdatePasswordCommandRequest : IRequest<Result>
    {
        public string PersonFin { get; set; } = default!;
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
    public class UpdatePasswordCommandRequestHandler(IPersonRepository iPersonRepository,IUnitOfWork iUnitOfWork):IRequestHandler<UpdatePasswordCommandRequest,Result>
    {
        public async Task<Result> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            var personFin = request.PersonFin;
            // var personFin = httpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            // if (string.IsNullOrWhiteSpace(personFin)){
            //        return Result.Fail("Unauthorized access !");
            // }

            var person = await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==personFin,cancellationToken);

            if (new PasswordHasher<PersonEntity>().VerifyHashedPassword(person,person.PasswordHash,request.CurrentPassword)==PasswordVerificationResult.Failed)
            {
                return Result.Fail("Current Password is wrong!");
            }

            var newHashedPassword = new PasswordHasher<PersonEntity>().HashPassword(person,request.NewPassword);
            person.PasswordHash = newHashedPassword;
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
            



        }
    }
}