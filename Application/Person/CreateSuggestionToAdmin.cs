using Application.Admin;
using Application.Interfaces;
using Domain.Suggestion;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Person;

public class CreateSuggestionToAdmin
{
    public class CreateSuggestionToAdminRequest : IRequest<Result>
    {
        public string Suggestion { get; set; } = default!;
    }
    public class CreateSuggestionToAdminRequestHandler(ISuggestionRepository iSuggestionRepository,IUnitOfWork iUnitOfWork,IHttpContextAccessor httpContextAccessor):IRequestHandler<CreateSuggestionToAdminRequest,Result>
    {
        public async Task<Result> Handle(CreateSuggestionToAdminRequest request, CancellationToken cancellationToken)
        {
            var personFin = httpContextAccessor.HttpContext?.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access !");
            var suggestion = request.Adapt<SuggestionEntity>();
            suggestion.PersonFin = personFin;
            await iSuggestionRepository.AddAsync(suggestion,cancellationToken);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}
