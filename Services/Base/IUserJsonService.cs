using Microsoft.AspNetCore.Mvc;

namespace backend.Services.Base
{
    public interface IUserJsonService
    {
        public Task<IActionResult> GetUserJsonAsync();
        public  Task<string> SaveUserJsonAsync(object jsonData);
    }
}
