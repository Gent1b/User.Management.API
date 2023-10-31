using AutoMapper;
using User.Management.API.Models;
using User.Management.API.Models.DTO;

namespace User.Management.API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Stay, StayDTO>().ReverseMap();
        }
    }
}
