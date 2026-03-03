using PortfolioTracker.Application.Interfaces;
using System.Security.Claims;

namespace PortfolioTracker
{
    public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
    {
        public string userId => accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
