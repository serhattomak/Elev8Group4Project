using System.Security.Claims;
using WiseProject.Business.Abstract;

namespace WiseProject.Business.Concrete
{
    public class CurrentUserManager : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetClaim(string claim)
        {
            var userType = _httpContextAccessor.HttpContext.User.FindFirst(claim);

            if (userType == null || !userType.Subject.IsAuthenticated)
                return "";
            return userType.Value;
        }

        public int UserId()
        {
            string userIdString = GetClaim(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            else
            {
                return 0; 
            }
        }
    }
}
