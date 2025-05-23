﻿using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        List<DishDto> GetAll(int restaurantId);
        DishDto GetById(int restaurantId, int dishId);
        void RemoveAll(int restaurantId);
    }

    public class DishService : IDishService
    {
        private RestaurantDbContext _restaurantDbContext;
        private IMapper _mapper;

        public DishService(RestaurantDbContext restaurantDbContext, IMapper mapper)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;

            _restaurantDbContext.Dishes.Add(dishEntity);
            _restaurantDbContext.SaveChanges();

            return dishEntity.Id;
        }
        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dish = _restaurantDbContext.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null || dish.RestaurantId != restaurantId)
                throw new NotFoundException("Dish not found");
            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }
        public List<DishDto> GetAll(int restaurantId) 
        {
            var restaurant = GetRestaurantById(restaurantId);

            List<DishDto> dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dishDtos;
        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            _restaurantDbContext.RemoveRange(restaurant.Dishes);
            _restaurantDbContext.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }
    }
}
