using AutoMapper;
using Server.Models;
using Server.Resource;

namespace Server.MappingProfiles
{
    public class StockMapper : Profile
    {
        public StockMapper()
        {
            CreateMap<Stock, StockResource>();
            CreateMap<StockResource, Stock>();
        }
    }
}
