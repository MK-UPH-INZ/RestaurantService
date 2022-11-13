using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RestaurantService.DTO;
using RestaurantService.Models;

namespace RestaurantService.Profiles
{
    public class RestaurantsProfile : Profile
    {
        public RestaurantsProfile()
        {
            // source -> target
            CreateMap<Restaurant, RestaurantReadDTO>();
            CreateMap<User, RestaurantUserDTO>();
            CreateMap<RestaurantCreateDTO, Restaurant>();
            CreateMap<RestaurantUpdateDTO, Restaurant>()
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<RestaurantReadDTO, RestaurantPublishedDTO>();
            CreateMap<Restaurant, RestaurantUpdatedDTO>();
        }
    }
}
