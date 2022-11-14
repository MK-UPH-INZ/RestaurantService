using AutoMapper;
using RestaurantService.DTO.Product;
using RestaurantService.DTO.Product.Events;
using RestaurantService.Models;

namespace RestaurantService.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductReadDTO>();
            CreateMap<Restaurant, ProductRestaurantDTO>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.ExternalId));
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, ProductPublishedDTO>();
            CreateMap<Product, ProductUpdatedDTO>();
        }
    }
}
