using ESourcing.Order.Application.Features.Orders.Commands.Create;
using ESourcing.Order.Application.Features.Orders.DTOs;
using ESourcing.Order.Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Order.Controller;

[Route("api/[controller]")]
[ApiController]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpGet("GetOrdersByUserName/{userName}", Name = "GetOrdersByUserName")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName) =>
        await mediator.Send(new GetOrdersBySellerUsernameQuery(userName)) is { } orders && orders.Any()
            ? Ok(orders)
            : NotFound();

    [HttpPost("Create")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderCreateCommand command)
    {
        var order = await mediator.Send(command);
        return CreatedAtRoute("GetOrdersByUserName", new { userName = order.SellerUserName }, order);
    }
}