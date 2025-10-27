using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Person;

public class GetBalanceQuery
{
    public class GetBalanceQueryRequest():IRequest<Result<GetBalanceQueryResponse>>
    {
    }

    public class GetBalanceQueryResponse
    {
        public decimal Balance { get; set; }
    }
    public class GetBalanceQueryRequestHandler(IPersonRepository iPersonRepository,IHttpContextAccessor httpContextAccessor):IRequestHandler<GetBalanceQueryRequest,Result<GetBalanceQueryResponse>>
    {
        public async Task<Result<GetBalanceQueryResponse>> Handle(GetBalanceQueryRequest request, CancellationToken cancellationToken)
        {
            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
            {
                return Result.Fail("Unauthorized access !");
            }
            
            

            var person =await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==personFin,cancellationToken);
            return Result.Ok(new GetBalanceQueryResponse()
            {
                Balance = person.Balance
            });
        }
    }
}