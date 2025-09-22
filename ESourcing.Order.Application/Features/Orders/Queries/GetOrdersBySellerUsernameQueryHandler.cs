using AutoMapper;
using ESourcing.Order.Application.Features.Orders.DTOs;
using ESourcing.Order.Domain.Repositories;
using MediatR;

namespace ESourcing.Order.Application.Features.Orders.Queries;

public class GetOrdersBySellerUsernameQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<GetOrdersBySellerUsernameQuery, IEnumerable<OrderResponse>>
{
    public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersBySellerUsernameQuery request,
        CancellationToken cancellationToken)
    {
        return mapper.Map<IEnumerable<OrderResponse>>(
            await orderRepository.GetOrdersBySellerUserName(request.UserName));
    }
}