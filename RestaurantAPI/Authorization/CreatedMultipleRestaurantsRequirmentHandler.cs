using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirmentHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirment>
    {
        private RestaurantDbContext _dbContext;

        public CreatedMultipleRestaurantsRequirmentHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirment requirement)
        {
            int UserId = int.Parse(context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            int CreatedRestaurantsByUser = _dbContext.Restaurants
                .Count(r => r.CreatedById == UserId);
            if(CreatedRestaurantsByUser >= requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
