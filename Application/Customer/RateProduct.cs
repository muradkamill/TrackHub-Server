using Application.Interfaces;
using Application.Product;
using Domain.Comment;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Customer;

public class RateProduct
{
    public static List<decimal> RateList =
    [
        0,
        0.5m,
        1,
        1.5m,
        2,
        2.5m,
        3,
        3.5m,
        4,
        4.5m,
        5
    ];
    public class RateCourierRequestValidation : AbstractValidator<RateProductRequest>
    {
        public RateCourierRequestValidation()
        {
            RuleFor(x => x.ProductRate).Must(x => RateList.Contains(x));
        }
    }
    public class RateProductRequest : IRequest<Result>
    {
        public int ProductId { get; set; }
        public decimal ProductRate { get; set; }
        public string? ProductComment { get; set; } = default!;



    }
    public class RateProductRequestHandler(ICommentRepository iCommentRepository,IProductRepository iProductRepository,IUnitOfWork iUnitOfWork):IRequestHandler<RateProductRequest,Result>
    {
        public async Task<Result> Handle(RateProductRequest request, CancellationToken cancellationToken)
        {
            // var personFin = iHttpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            // if (string.IsNullOrWhiteSpace(personFin))
            //     return Result.Fail("Unauthorized access!");
            var personFin = "1111111";
            var product = await iProductRepository.FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);
            product.RateQuantity += 1;
            if (request.ProductComment != null)
            {
                var comment = new CommentEntity()
                {
                    Comment = request.ProductComment,
                    ProductId = request.ProductId,
                    CreateAt = DateTime.UtcNow,
                    PersonFin = personFin
                };
                await iCommentRepository.AddAsync(comment,cancellationToken);
            }
            var totalRate = (product.ProductRate*(product.RateQuantity-1) + request.ProductRate) / product.RateQuantity;
            product.ProductRate = totalRate;
            iProductRepository.Update(product);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();

        }
    }
}