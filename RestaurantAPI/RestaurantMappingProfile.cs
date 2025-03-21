using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(r => r.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(r => r.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(r => r.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(m => m.Address, c => c.MapFrom(dto => new Address() { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));
            
            CreateMap<ModificationRestaurantDto, Restaurant>();
        }
    }
}
