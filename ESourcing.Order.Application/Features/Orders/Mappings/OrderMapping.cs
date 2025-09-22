using AutoMapper;
using ESourcing.Order.Application.Features.Orders.Commands.Create;
using ESourcing.Order.Application.Features.Orders.DTOs;

namespace ESourcing.Order.Application.Features.Orders.Mappings;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<Domain.Entities.Order, OrderCreateCommand>().ReverseMap();
        CreateMap<Domain.Entities.Order, OrderResponse>().ReverseMap();
    }
}