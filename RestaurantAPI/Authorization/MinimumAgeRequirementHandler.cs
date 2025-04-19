using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;
        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {           
            var userDateOfBirthClaim = context.User.FindFirst(c => c.Type == "DateOfBirth");
            if (userDateOfBirthClaim == null)
            {
                return Task.CompletedTask;
            }
            var dateOfBirth = DateTime.Parse(userDateOfBirthClaim.Value);
            // dalsza część...

            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            _logger.LogInformation($"User {userEmail} with date of birth {dateOfBirth} is being evaluated for minimum age requirement of {requirement.MinimumAge} years.");

            if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
