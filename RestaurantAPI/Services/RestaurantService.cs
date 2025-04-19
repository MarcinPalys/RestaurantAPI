using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int Id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(CreateRestaurantDto dto, int userId);
        void Delete(int id, ClaimsPrincipal user);
        void Update(int id, ModificationRestaurantDto dto, ClaimsPrincipal user);
    }
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;

        public RestaurantService(RestaurantDbContext restaurantDbContext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
        }
        public void Update(int id, ModificationRestaurantDto dto, ClaimsPrincipal user)
        {
            

            var restaurant = _restaurantDbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);
            if (restaurant is null)
                throw new NotFoundException("Not found restaurant");

            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, new ResourceOperationRequirment(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded) 
            {
                throw new ForbidException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;          
            restaurant.HasDelivery = dto.HasDelivery;
            
            _restaurantDbContext.SaveChanges();
        }
        public void Delete(int id, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");

            var restaurant = _restaurantDbContext
              .Restaurants              
              .FirstOrDefault(r => r.Id == id);
            if (restaurant is null) 
                throw new NotFoundException("Not found restaurant");

            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, new ResourceOperationRequirment(ResourceOperation.Update)).Result;


            _restaurantDbContext.Restaurants.Remove(restaurant);
            _restaurantDbContext.SaveChanges();
        }
        public RestaurantDto GetById(int Id)
        {           
            var restaurant = _restaurantDbContext
               .Restaurants
               .Include(r => r.Address)
               .Include(r => r.Dishes)
               .FirstOrDefault(r => r.Id == Id);
            if (restaurant is null)
                throw new NotFoundException("Not found restaurant");

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
        public int Create(CreateRestaurantDto dto, int userId)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = userId;
            _restaurantDbContext.Restaurants.Add(restaurant);
            _restaurantDbContext.SaveChanges();

            return restaurant.Id;
        }
    }
}
