using Application.Interfaces;
using Application.Person;
using Domain.Auth;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace Application.Auth;

public class LoginCommand()
{
    public class RegisterValidation : AbstractValidator<LoginRequest>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Fin)
                .Length(7).WithMessage("FIN must be 7 character")
                .NotEmpty().WithMessage("FIN cannot be empty");
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password must be minimum 8 character").NotEmpty().WithMessage("Password  cannot be empty");
            

        }
    }
    public class LoginRequest():IRequest<Result<LoginResponse>>
    {
        public string Fin { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginResponse()
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }

    public class LoginCommandHandler(IPersonRepository iPersonRepository,IUnitOfWork iUnitOfWork,IConfiguration configuration)
        : IRequestHandler<LoginRequest, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            if (!await iPersonRepository.AnyAsync(x => x.Fin == request.Fin, cancellationToken))
            {
                return Result.Fail("This Person is not Registered,Please Register and Login Later");
            }
    
            var person = await iPersonRepository.FirstOrDefaultAsync(x => x.Fin == request.Fin, cancellationToken);
    
            if (new PasswordHasher<PersonEntity>().VerifyHashedPassword(person, person.PasswordHash,
                    request.Password) == PasswordVerificationResult.Failed)
            {
                return Result.Fail("Password is wrong!");
            }

            person.RefreshToken = Methods.CreateRefreshToken();
            person.RefreshTokenExpiryDate=DateTime.UtcNow.AddDays(7);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(new LoginResponse()
            {
                AccessToken = Methods.CreateAccessToken(configuration,person),
                RefreshToken = person.RefreshToken
            });

        }

        
            
        }
    }