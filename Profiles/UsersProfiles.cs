using AutoMapper;
using IdentityService;
using RestaurantService.DTO.User.Events;
using RestaurantService.Models;

namespace RestaurantService.Profiles
{
    public class UsersProfiles : Profile
    {
        public UsersProfiles()
        {
            CreateMap<UserPublishedDTO, User>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserUpdatedDTO, User>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcUserModel, User>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.UserRestaurants, opt => opt.Ignore());
        }
    }
}
