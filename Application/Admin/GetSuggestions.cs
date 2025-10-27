using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.Admin;

public class GetSuggestions
{
    public enum SuggestionStatusEnum
    {
        Pending,
        Seen
        
    }

    public class GetSuggestionsRequest : IRequest<Result<IQueryable<GetSuggestionsResponse>>>;
    public class GetSuggestionsResponse
    {
        public string PersonFin { get; set; } = default!;
        public string Suggestion { get; set; } = default!;
    }
    public class GetSuggestionsRequestHandler(ISuggestionRepository iSuggestionRepository):IRequestHandler<GetSuggestionsRequest,Result<IQueryable<GetSuggestionsResponse>>>
    {
        public async Task<Result<IQueryable<GetSuggestionsResponse>>> Handle(GetSuggestionsRequest request, CancellationToken cancellationToken)
        {
            var suggestions =
                iSuggestionRepository.Where(x => x.SuggestionStatus == nameof(SuggestionStatusEnum.Pending));
            if (suggestions is null)
            {
                return Result.Fail("Pending Suggestion is not found!");
            }

            var response = suggestions.ProjectToType<GetSuggestionsResponse>();
            return Result.Ok(await Task.FromResult(response));

        }
    }
}