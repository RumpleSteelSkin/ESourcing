using ESourcing.Order.Application.Features.Orders.DTOs;
using MediatR;

namespace ESourcing.Order.Application.Features.Orders.Queries;

public class GetOrdersBySellerUsernameQuery(string userName) : IRequest<IEnumerable<OrderResponse>>
{
    public string UserName { get; set; } = userName;
}