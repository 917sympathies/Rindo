using AutoMapper;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectOnReturnDto>()
            .ReverseMap()
            .ForMember(p => p.ChatId,
                condition =>
                    condition.MapFrom(p => p.Chat.Id))
            .ForMember(p => p.OwnerId,
                condition =>
                    condition.MapFrom(p => p.Owner.Id));
        CreateMap<Project, ProjectOnCreateDto>()
            .ReverseMap();
        CreateMap<UserDtoSignUp, User>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Project, ProjectSettingsDto>();
        CreateMap<Project, ProjectInfoSidebar>();
        CreateMap<Stage, StageOnCreateDto>().ReverseMap();
        CreateMap<Project, ProjectInfoHeader>().ReverseMap();
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleDtoOnCreate>().ReverseMap();
    }
}