using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RestaurantService.DTO.Restaurant;
using RestaurantService.DTO.Restaurant.Events;
using RestaurantService.Models;

namespace RestaurantService.Profiles
{
    public class RestaurantsProfile : Profile
    {
        public RestaurantsProfile()
        {
            // source -> target
            CreateMap<Restaurant, RestaurantReadDTO>();
            CreateMap<User, RestaurantUserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
            CreateMap<RestaurantCreateDTO, Restaurant>();
            CreateMap<RestaurantUpdateDTO, Restaurant>()
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<RestaurantReadDTO, RestaurantPublishedDTO>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id));
            CreateMap<Restaurant, RestaurantUpdatedDTO>();
            CreateMap<Restaurant, GrpcRestaurantModel>()
                .ForMember(dest => dest.RestaurantId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.ExternalId));
        }
    }
}
