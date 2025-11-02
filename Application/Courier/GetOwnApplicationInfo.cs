using Application.Auth;
using Application.Interfaces;
using Application.Person;
using Domain.Auth;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Courier;

public class GetOwnApplicationInfo
{
    public class GetOwnApplicationInfoRequest : IRequest<Result<GetOwnApplicationInfoResponse>>
    {
    }

    public class GetOwnApplicationInfoResponse
    {
        public string Fin { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string SurName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string VehicleType { get; set; } = default!;
        public string CvUrl { get; set; } = default!;
        public string ApplicationStatus { get; set; } = default!;
    }
    
    
    public class GetOwnApplicationInfoRequestHandler(IPersonRepository iPersonRepository,IHttpContextAccessor httpContextAccessor):IRequestHandler<GetOwnApplicationInfoRequest,Result<GetOwnApplicationInfoResponse>>
    {
        public async Task<Result<GetOwnApplicationInfoResponse>> Handle(GetOwnApplicationInfoRequest request, CancellationToken cancellationToken)
        {
            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");

            var person=await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==personFin,cancellationToken);
            var application = person.Adapt<GetOwnApplicationInfoResponse>();
            return Result.Ok(application);
        }
    }
}