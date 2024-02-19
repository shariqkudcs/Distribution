using AutoMapper;
using Server.Models;
using Server.Resource;

namespace Server.MappingProfiles
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, CustomerResource>();
            CreateMap<CustomerResource, Customer>();
        }
    }
}
