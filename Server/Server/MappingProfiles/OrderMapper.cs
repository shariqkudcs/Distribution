using AutoMapper;
using Server.Models;
using Server.Resource;

namespace Server.MappingProfiles
{
    public class OrderMapper: Profile
    {
        public OrderMapper()
        {
            CreateMap<Order, OrderResource>();
            CreateMap<OrderResource, Order>();
        }
    }
}
