using backend.Data;
using backend.Services.Base;

namespace backend.Services
{
    public class AuthService : IAuthService
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(ApplicationDbContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }


        public int? GetAuthenticatedUserId()
        {

            var claims = _httpContextAccessor.HttpContext.User.Claims;
            foreach (var claim in claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "id");

            if (userIdClaim == null)
            {
                Console.WriteLine("User ID claim is missing.");
                return null;
            }

            if (int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }

            Console.WriteLine($"Failed to parse User ID from claim: {userIdClaim.Value}");
            return null;
        }

    }
}
