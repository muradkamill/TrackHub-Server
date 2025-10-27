using Application.Interfaces;
using Application.Person;
using Domain.Auth;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.Admin;

public class GetPendingApplications
{
    public class GetPendingApplicationsRequest() : IRequest<Result<IQueryable<GetPendingApplicationsResponse>>>;

    public class GetPendingApplicationsResponse()
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public AddressEntity Address { get; set; } = default!;
        public string? ImageUrl { get; set; } = default!;
        public string VehicleType { get; set; } = default!;
        public string CvUrl { get; set; } = default!;
    }
    public class GetPendingApplicationsRequestHandler(IPersonRepository iPersonRepository):IRequestHandler<GetPendingApplicationsRequest,Result<IQueryable<GetPendingApplicationsResponse>>>
    {
        public async Task<Result<IQueryable<GetPendingApplicationsResponse>>> Handle(GetPendingApplicationsRequest request, CancellationToken cancellationToken)
        {
            var people =iPersonRepository.Where(x => x.Role == "Courier" && x.ApplicationStatus=="Pending");
            if (people is null)
            {
                return Result.Fail("No pending courier application!");
            }

            var response = people.ProjectToType<GetPendingApplicationsResponse>();
            return Result.Ok(await Task.FromResult(response));
        }
    }
}