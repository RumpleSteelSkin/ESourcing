using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ESourcing.Order.Application.Pipelines;

public class PerformanceBehaviour<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        stopwatch.Stop();
        if (stopwatch.ElapsedMilliseconds > 0)
            logger.LogWarning("{RequestType} : {StopwatchElapsedMilliseconds} MS", request.GetType(),
                stopwatch.ElapsedMilliseconds);
        return response;
    }
}