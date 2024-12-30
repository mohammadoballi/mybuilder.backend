using Microsoft.AspNetCore.Mvc;

namespace backend.Services.Base
{
    public interface IAdminService
    {
        public Task<IActionResult> GetUserJsonAsync();

    }
}
