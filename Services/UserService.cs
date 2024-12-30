using backend.Data;
using backend.Model;
using backend.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _auth;


        public UserService(ApplicationDbContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IAuthService auth)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _auth = auth;
        }


        public User getUser()
        {
            var userId = _auth.GetAuthenticatedUserId();

            var user = _context.Users.Where(u=>u.Id == userId).FirstOrDefault();
            if(user != null)
            {
                return user;
            }
            return null;
        }

       

       


    }
}
