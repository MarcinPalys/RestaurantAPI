using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : Controller
    {
        private IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        [HttpDelete]
        //Zrobic usuwanie konkretnego dania z konkretnej restauracji przez id
        public ActionResult Delete([FromRoute] int restaurantId)
        {
            _dishService.RemoveAll(restaurantId);

            return NoContent();

        }
        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            int newDishId = _dishService.Create(restaurantId, dto);
            
            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }
        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            DishDto dish = _dishService.GetById(restaurantId, dishId);

            return Ok(dish);
        }
        [HttpGet]
        public ActionResult<List<DishDto>> GetAll([FromRoute] int restaurantId)
        {
            List<DishDto> dishs = _dishService.GetAll(restaurantId);

            return Ok(dishs);
        }
    }
}
