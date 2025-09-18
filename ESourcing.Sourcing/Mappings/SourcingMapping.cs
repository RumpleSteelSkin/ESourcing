using AutoMapper;
using ESourcing.Sourcing.Entities;
using EventBusRabbitMQ.Events.Concretes;

namespace ESourcing.Sourcing.Mappings;

public class SourcingMapping : Profile
{
    public SourcingMapping()
    {
        CreateMap<Bid, OrderCreateEvent>().ReverseMap();
    }
}