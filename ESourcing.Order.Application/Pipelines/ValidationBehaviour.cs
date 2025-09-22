using FluentValidation;
using MediatR;

namespace ESourcing.Order.Application.Pipelines;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = (await Task.WhenAll(validators.Select(v =>
                v.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken))))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();
        return failures.Any() ? throw new ValidationException(failures) : await next(cancellationToken);
    }
}