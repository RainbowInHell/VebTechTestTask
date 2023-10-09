namespace VebTechTestTask.AutoMapperConfig
{
    using AutoMapper;

    using DAL.Entities;
    
    using Requests.User;
    
    using ViewModels;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserLinks.Select(ul => ul.Role.Name)))
                .ReverseMap();
            
            CreateMap<AddUserRequest, User>()
                .ReverseMap();
            
            CreateMap<UpdateUserRequest, User>()
                .ReverseMap();
        }
    }
}