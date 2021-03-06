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
            CreateMap<RestaurantCreateDTO, Restaurant>();
            CreateMap<RestaurantUpdateDTO, Restaurant>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<RestaurantReadDTO, RestaurantPublishedDTO>();
        }
    }
}
