using System.Transactions;
using MediatR;

namespace ESourcing.Order.Application.Pipelines;

public class TransactionalBehaviour<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);
        var response = await next(cancellationToken);
        transaction.Complete();
        return response;
    }
}