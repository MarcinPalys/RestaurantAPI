using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int Id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(CreateRestaurantDto dto);
        bool Delete(int id);
        bool Update(int id, ModificationRestaurantDto dto);
    }
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        public RestaurantService(RestaurantDbContext restaurantDbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public bool Update(int id, ModificationRestaurantDto dto)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);
            if (restaurant is null) return false;
            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;          
            restaurant.HasDelivery = dto.HasDelivery;
            
            _restaurantDbContext.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");

            var restaurant = _restaurantDbContext
              .Restaurants              
              .FirstOrDefault(r => r.Id == id);
            if (restaurant is null) return false;
            _restaurantDbContext.Restaurants.Remove(restaurant);
            _restaurantDbContext.SaveChanges();
            return true;
        }
        public RestaurantDto GetById(int Id)
        {           
            var restaurant = _restaurantDbContext
               .Restaurants
               .Include(r => r.Address)
               .Include(r => r.Dishes)
               .FirstOrDefault(r => r.Id == Id);
            if (restaurant is null) return null;

            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;
        }
        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _restaurantDbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList();
            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDto;
        }
        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _restaurantDbContext.Restaurants.Add(restaurant);
            _restaurantDbContext.SaveChanges();

            return restaurant.Id;
        }
    }
}
