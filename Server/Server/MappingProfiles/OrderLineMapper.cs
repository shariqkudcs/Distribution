using AutoMapper;
using Server.Models;
using Server.Resource;

namespace Server.MappingProfiles
{
    public class OrderLineMapper : Profile
    {
        public OrderLineMapper()
        {
            CreateMap<OrderLine, OrderLineResource>();
            CreateMap<OrderLineResource, OrderLine>();
        }
    }
}
