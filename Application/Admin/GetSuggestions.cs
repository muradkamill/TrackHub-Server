using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;

namespace Application.Admin;

public class GetSuggestions
{
    public class GetSuggestionsRequest : IRequest<Result<List<GetSuggestionsResponse>>>;
    public class GetSuggestionsResponse
    {
        public string PersonFin { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string SurName { get; set; } = default!;
        public string Suggestion { get; set; } = default!;
    }
    public class GetSuggestionsRequestHandler(ISuggestionRepository iSuggestionRepository):IRequestHandler<GetSuggestionsRequest,Result<List<GetSuggestionsResponse>>>
    {
        public async Task<Result<List<GetSuggestionsResponse>>> Handle(GetSuggestionsRequest request, CancellationToken cancellationToken)
        {
            var suggestions =
                iSuggestionRepository.GetAll().ToList();

            var response = suggestions.Adapt<List<GetSuggestionsResponse>>();
            return await Task.FromResult(Result.Ok(response));

        }
    }
}