using AutoMapper;
using Server.Models;
using Server.Response;

namespace Server.MappingProfiles
{
    public class WarehouseMapper : Profile
    {
        public WarehouseMapper()
        {
            CreateMap<Warehouse, WarehouseResource>();
            CreateMap<WarehouseResource, Warehouse>();
        }
    }
}
