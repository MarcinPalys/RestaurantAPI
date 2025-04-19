using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirment : IAuthorizationRequirement
    {
        public CreatedMultipleRestaurantsRequirment(int _MinimumRestaurantsCreated)
        {
            MinimumRestaurantsCreated = _MinimumRestaurantsCreated;
        }
        public int MinimumRestaurantsCreated { get; set; }
    }
}
