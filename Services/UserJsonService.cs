using System.IO;
using System.Threading.Tasks;
using backend.Data;
using backend.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using backend.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace backend.Services
{
    [Authorize]
    public class UserJsonService : IUserJsonService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _auth;

        public UserJsonService(ApplicationDbContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IAuthService auth)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _auth = auth;

        }



        public async Task<string> SaveUserJsonAsync(object jsonData)
        {
            var userId = _auth.GetAuthenticatedUserId();

            if (userId == null)
            {
                Console.WriteLine("No authenticated user found.");
                return null;
            }

            if (string.IsNullOrEmpty(_env.WebRootPath))
            {
                Console.WriteLine("WebRootPath is not configured.");
                return null;
            }

            var fileName = $"{Guid.NewGuid()}.json";
            var filePath = Path.Combine(_env.WebRootPath, "userJsons", fileName);

            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString;
                if (jsonData is JsonElement jsonElement)
                {
                    jsonString = jsonElement.ToString();
                }
                else
                {
                    jsonString = JsonConvert.SerializeObject(jsonData);
                }

                await File.WriteAllTextAsync(filePath, jsonString);

                var userJsonData = new UserJsonData
                {
                    UserId = userId.Value,
                    FilePath = filePath
                };

                _context.UserJsonData.Add(userJsonData);
                await _context.SaveChangesAsync();

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<IActionResult> GetUserJsonAsync()
        {
            var userId = _auth.GetAuthenticatedUserId();

            if (userId == null)
                return new UnauthorizedObjectResult(new { Message = "User is not authenticated." });

            var userJsonDataList = await _context.UserJsonData
                .Where(uj => uj.UserId == userId)
                .ToListAsync();

            if (userJsonDataList == null || !userJsonDataList.Any())
                return new NotFoundObjectResult(new { Message = "No JSON files found for the authenticated user." });

            var jsonFiles = new List<object>();

            foreach (var userJsonData in userJsonDataList)
            {
                if (File.Exists(userJsonData.FilePath))
                {
                    var jsonData = await File.ReadAllTextAsync(userJsonData.FilePath);
                    jsonFiles.Add(new { FilePath = userJsonData.FilePath, Data = jsonData });
                }
            }

            if (!jsonFiles.Any())
                return new NotFoundObjectResult(new { Message = "No JSON files found on the server." });

            return new OkObjectResult(jsonFiles); 
        }

    }
}
