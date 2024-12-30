using backend.Data;
using backend.Model;
using backend.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class AdminService:IAdminService
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AdminService(IAuthService authService,ApplicationDbContext context) {
            _authService = authService;
            _context = context;
        }

        private User getAdmin(int? id)
        {
           var user =  _context.Users.Where(u => u.Id == id).First();
            if (user == null)
            {
                throw new Exception("user not found");
            }

            if(!user.isAdmin)
            {
                throw new Exception("user not admin");

            }

            return user;
        }

        public async Task<IActionResult> GetUserJsonAsync()
        {
            var userId = _authService.GetAuthenticatedUserId();

            var user = getAdmin(userId);

            var userJsonDataList = await _context.UserJsonData.ToListAsync();

            if (userJsonDataList == null || !userJsonDataList.Any())
                return new NotFoundObjectResult(new { Message = "No JSON files found for the authenticated user." });

            var jsonFiles = new List<object>();

            foreach (var userJsonData in userJsonDataList)
            {
                if (File.Exists(userJsonData.FilePath))
                {
                    var username = _context.Users.Where(u=>u.Id==userJsonData.UserId).First();
                    var jsonData = await File.ReadAllTextAsync(userJsonData.FilePath);
                    jsonFiles.Add(new {UserName=username.FirstName +" "+user.LastName , FilePath = userJsonData.FilePath, Data = jsonData });
                }
            }

            if (!jsonFiles.Any())
                return new NotFoundObjectResult(new { Message = "No JSON files found on the server." });

            return new OkObjectResult(jsonFiles);
        }



    }
}
