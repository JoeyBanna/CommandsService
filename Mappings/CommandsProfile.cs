using AutoMapper;

namespace CommandsService.Mappings
{
    public class CommandsProfile : Profile
    { 
        public CommandsProfile()
        {
            CreateMap<Models.Platform, DTOs.PlatformReadDTO>();
            CreateMap<DTOs.CommandCreateDTO, Models.Command>();
            CreateMap<Models.Command, DTOs.CommandReadDTO>();
            CreateMap<DTOs.PlatformPublishedDTO, Models.Platform>().ForMember(dest => dest.ExternalId, opt=> opt.MapFrom(src=>src.Id));
        }

    }
}

