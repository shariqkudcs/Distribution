using AutoMapper;
using Server.Models;
using Server.Resource;

namespace Server.MappingProfiles
{
    public class DistrictMapper : Profile
    {
        public DistrictMapper()
        {
            CreateMap<District, DistrictResource>();
            CreateMap<DistrictResource, District>();
        }
    }
}
