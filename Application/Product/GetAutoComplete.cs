using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Product;

public class GetAutoComplete
{
    public class GetAutoCompleteResponse
    {
        public string ProductNameSuggestion { get; set; } = default!;

    }
    public class GetAutoCompleteRequest : IRequest<Result<List<GetAutoCompleteResponse>>>
    {
        public string? Text { get; set; } = default!;

    }
    public class GetAutoCompleteRequestHandler(IProductRepository iProductRepository) : IRequestHandler<GetAutoCompleteRequest,Result<List<GetAutoCompleteResponse>>>
    {
        public async Task<Result<List<GetAutoCompleteResponse>>> Handle(GetAutoCompleteRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return Result.Ok(new List<GetAutoCompleteResponse>());
            }
            var products = iProductRepository.Where(x => x.Name.ToLower()
                    .StartsWith(request.Text.ToLower()))
                .Select(x => new GetAutoCompleteResponse()
                {
                    ProductNameSuggestion = x.Name
                }).ToList();
            return await Task.FromResult(Result.Ok(products));
        }
    }



}