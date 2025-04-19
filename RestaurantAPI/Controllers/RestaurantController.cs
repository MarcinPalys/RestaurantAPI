using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Authorize]
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : Controller
    {
        
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] ModificationRestaurantDto dto)
        {          

            _restaurantService.Update(id, dto);
            
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id) 
        { 
            _restaurantService.Delete(id);
           
            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Menager")]
        public ActionResult CreateRestaurant([FromBody]CreateRestaurantDto dto) 
        {              
            int result = _restaurantService.Create(dto);
            return Created($"api/restaurant/{result}",null);
        }

        [HttpGet]
        [Authorize(Policy = "CreatedMinimumTwoRestaurants")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {           
            return Ok(_restaurantService.GetAll());
        }
        [HttpGet("{Id}")]
        [AllowAnonymous]
        public ActionResult<RestaurantDto> GetById([FromRoute] int Id)
        {
            var restaurantDto = _restaurantService.GetById(Id);
            
            return Ok(restaurantDto);
        }
    }
}
