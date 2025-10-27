

using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Behavior;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var errorDictionary = validators
            .Select(s => s.Validate(context))
            .SelectMany(s => s.Errors)
            .Where(s => s != null)
            .GroupBy(
                s => s.PropertyName,
                s => s.ErrorMessage, (propertyName, errorMessage) => new
                {
                    Key = propertyName,
                    Values = errorMessage.Distinct().ToArray()
                })
            .ToDictionary(s => s.Key, s => s.Values[0]);

        if (errorDictionary.Any())
        {
            var errorMessages = errorDictionary.Select(s => s.Value).ToList();

            if (typeof(TResponse) == typeof(Result))
            {
                object result = Result.Fail(errorMessages);
                return (TResponse)result!;
            }

            var resultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments().First());
            var failMethod = resultType.GetMethod("Fail", new[] { typeof(IEnumerable<string>) });
            var resultInstance = failMethod?.Invoke(null, new object[] { errorMessages });
            return (TResponse)resultInstance!;
        }

        return await next(cancellationToken);
    }
}