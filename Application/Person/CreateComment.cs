using System.Globalization;
using Application.Interfaces;
using Domain.Comment;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Person;

public class CreateComment
{
    public class CreateCommentRequest : IRequest<Result>
    {
        public int ProductId { get; set; }
        public string Comment { get; set; } = default!;
    }
    public class CreateCommentRequestHandler(ICommentRepository iCommentRepository,IHttpContextAccessor iHttpContextAccessor,IUnitOfWork iUnitOfWork):IRequestHandler<CreateCommentRequest,Result>
    {
        public async Task<Result> Handle(CreateCommentRequest request, CancellationToken cancellationToken)
        {
            var personFin = iHttpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(personFin))
                return Result.Fail("Unauthorized access!");
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            var comment = new CommentEntity()
            {
                PersonFin = personFin,
                Comment =textInfo.ToTitleCase(request.Comment.ToLower()) ,
                ProductId = request.ProductId,
                CreateAt = DateTime.UtcNow,
            };
            await iCommentRepository.AddAsync(comment, cancellationToken);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();


        }
    }
}