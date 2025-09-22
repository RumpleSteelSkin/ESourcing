using AutoMapper;
using ESourcing.Order.Application.Features.Orders.DTOs;
using ESourcing.Order.Domain.Repositories;
using MediatR;

namespace ESourcing.Order.Application.Features.Orders.Commands.Create;

public class OrderCreateCommandHandler(
    IOrderRepository orderRepository,
    IMapper mapper) : IRequestHandler<OrderCreateCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = mapper.Map<Domain.Entities.Order>(request);
        return orderEntity == null
            ? throw new ApplicationException("Entity could not be mapped!")
            : mapper.Map<OrderResponse>(await orderRepository.AddAsync(orderEntity));
    }
}