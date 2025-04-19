using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? UserId { get; }
    }

    public class UserContextService : IUserContextService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int? UserId => User is null ? null : int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);


    }
}
