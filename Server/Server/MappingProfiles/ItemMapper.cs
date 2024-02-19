using AutoMapper;
using Server.Models;
using Server.Resource;

namespace Server.MappingProfiles
{
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<Item, ItemResource>();
            CreateMap<ItemResource, Item>();
        }
    }
}
