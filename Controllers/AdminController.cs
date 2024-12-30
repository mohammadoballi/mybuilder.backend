using backend.Services.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _admin;

        public AdminController(IAdminService admin)
        {
            _admin = admin;
        }

        [HttpGet("get-jsons")]
        public async Task<IActionResult> GetJsons() {
            try
            {

                var result = await _admin.GetUserJsonAsync();
                return result;

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            }
    }
}
