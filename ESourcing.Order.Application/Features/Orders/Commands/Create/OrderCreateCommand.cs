using ESourcing.Order.Application.Features.Orders.DTOs;
using MediatR;

namespace ESourcing.Order.Application.Features.Orders.Commands.Create;

public class OrderCreateCommand : IRequest<OrderResponse>
{
    public string? AuctionId { get; set; }
    public string? SellerUserName { get; set; }
    public string? ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}