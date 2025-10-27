using Application.Interfaces;
using Application.Person;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Auth;

public class RefreshCommand
{
    public class RefreshRequest() : IRequest<Result<RefreshResponse>>
    {
        public string RefreshToken { get; set; } = default!;
    }
    public class RefreshResponse() 
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!; 
    }
    
    public class RefreshCommandHandler(IPersonRepository iPersonRepository,IConfiguration configuration,IUnitOfWork iUnitOfWork):IRequestHandler<RefreshRequest,Result<RefreshResponse>>
    {
        public async Task<Result<RefreshResponse>> Handle(RefreshRequest request, CancellationToken cancellationToken)
        {
            var person =await iPersonRepository.FirstOrDefaultAsync(x=>x.RefreshToken==request.RefreshToken, cancellationToken);

            if (!await iPersonRepository.AnyAsync(x=>x.Fin==person.Fin,cancellationToken))
            {
                return Result.Fail("This Person is not exist!");
            }

            if (request.RefreshToken != person.RefreshToken)
            {
                return Result.Fail("Refresh Token is not correct!");
            }


            person.RefreshToken = Methods.CreateRefreshToken();
            person.RefreshTokenExpiryDate=DateTime.UtcNow.AddDays(7);

            
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(new RefreshResponse()
            {
                AccessToken = Methods.CreateAccessToken(configuration,person),
                RefreshToken = person.RefreshToken
            });
        }
    }
}