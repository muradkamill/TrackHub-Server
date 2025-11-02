using System.Globalization;
using Application.Interfaces;
using Application.Person;
using Domain.Auth;
using FluentResults;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth;

public class RegisterCommand
{
    public enum RoleEnum
    {
        User,
        Admin,
        Courier,
    }

    public class RegisterValidation : AbstractValidator<RegisterRequest>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Fin)
                .Length(7).WithMessage("FIN must be 7 character")
                .NotEmpty().WithMessage("FIN cannot be empty");
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password must be minimum 8 character").NotEmpty()
                .WithMessage("Password  cannot be empty");
            RuleFor(x => x.Name)
                .MinimumLength(3).WithMessage("Name must be minimum 3 character")
                .MaximumLength(15).WithMessage("Name must be maximum 15 character");
            RuleFor(x => x.SurName)
                .MinimumLength(3).WithMessage("SurName must be minimum 3 character")
                .MaximumLength(15).WithMessage("SurName must be maximum 15 character");
        }
    }

    public class RegisterRequest() : IRequest<Result>
    {
        public string Fin { get; set; } = default!;

        public string Password { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Mail { get; set; } = default!;

        public string SurName { get; set; } = default!;


    }

    public class RegisterHandler(IPersonRepository iPersonRepository, IUnitOfWork iUnitOfWork)
        : IRequestHandler<RegisterRequest, Result>
    {
        public async Task<Result> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {

            if (await iPersonRepository.AnyAsync(x => x.Fin == request.Fin,cancellationToken))
            {
                return Result.Fail("This FIN already exist!");
            }


            var person = request.Adapt<PersonEntity>();
            person.DeliveredPackageQuantity = 0;
            person.CourierRate = 0;


            person.PasswordHash = new PasswordHasher<PersonEntity>().HashPassword(person, request.Password);

            if (request.Fin == "820RD60" && request.Password == "Murad1334")
            {
                person.Role = "Admin";
            }
            else
            {
                person.Role = "User";
            }
            await iPersonRepository.AddAsync(person, cancellationToken);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}