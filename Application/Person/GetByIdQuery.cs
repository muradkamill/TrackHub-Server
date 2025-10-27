using Application.Interfaces;
using Domain.Auth;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Person;

public class GetByIdQuery
{
    public class GetByHeaderRequest() :IRequest<Result<GetByIdResponse>>
    {
    }
    public class GetByIdResponse()
    {
        public string Fin { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public AddressEntity Address { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string SurName { get; set; } = default!;
        public string Mail { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;


    }
    
    internal class GetByHeaderHandler(IPersonRepository iPersonRepository,IHttpContextAccessor iHttpContextAccessor):IRequestHandler<GetByHeaderRequest,Result<GetByIdResponse>>
    {
        public async Task<Result<GetByIdResponse>> Handle(GetByHeaderRequest request, CancellationToken cancellationToken)
        {
            var fin = iHttpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            if (!await iPersonRepository.AnyAsync(x=>x.Fin==fin,cancellationToken) || fin is null)
            {
                return Result.Fail("User is not found!");
            }

            var person = await iPersonRepository.FirstOrDefaultAsync(x => x.Fin == fin, cancellationToken);
            var response=person.Adapt<GetByIdResponse>();
            return Result.Ok(response);
        }
    }
}