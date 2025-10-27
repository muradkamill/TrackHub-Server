using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Product;

public class GetProductComments
{
    public class GetProductCommentsRequest : IRequest<Result<List<GetProductCommentsResponse>>>
    {
        public int ProductId { get; set; }

    }
    public class GetProductCommentsResponse
    {
        public int Id { get; set; }
        public string PersonFin { get; set; } = default!;
        public int ProductId { get; set; }
        public string Comment { get; set; } = default!;
        public DateTime CreateAt { get; set; } = default!;


    }

    public class GetProductCommentsRequestHandler(ICommentRepository iCommentRepository) : IRequestHandler<GetProductCommentsRequest,Result<List<GetProductCommentsResponse>>>
    {
        public async Task<Result<List<GetProductCommentsResponse>>> Handle(GetProductCommentsRequest request, CancellationToken cancellationToken)
        {
            var comments = iCommentRepository.Where(x => x.ProductId == request.ProductId).ToList();
            var res = comments.Adapt<List<GetProductCommentsResponse>>();
            return await Task.FromResult(Result.Ok(res));
        }
    }
}