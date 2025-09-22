using AutoMapper;
using ESourcing.Order.Application.Features.Orders.Commands.Create;
using EventBusRabbitMQ.Events.Concretes;

namespace ESourcing.Order.Mappings;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<OrderCreateEvent, OrderCreateCommand>().ReverseMap();
    }
}