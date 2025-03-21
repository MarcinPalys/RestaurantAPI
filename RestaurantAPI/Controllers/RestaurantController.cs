using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isUpdated = _restaurantService.Update(id, dto);
            if (!isUpdated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id) 
        { 
            bool result = _restaurantService.Delete(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
        [HttpPost]
        public ActionResult CreateRestaurant([FromBody]CreateRestaurantDto dto) 
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int result = _restaurantService.Create(dto);
            return Created($"api/restaurant/{result}",null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {           
            return Ok(_restaurantService.GetAll());
        }
        [HttpGet("{Id}")]
        public ActionResult<RestaurantDto> GetById([FromRoute] int Id)
        {
            var restaurantDto = _restaurantService.GetById(Id);
            if (restaurantDto is null)
            {
                return NotFound();
            }
            return Ok(restaurantDto);
        }
    }
}
