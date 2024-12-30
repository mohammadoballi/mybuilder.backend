using backend.Services;
using backend.Request;
using Microsoft.AspNetCore.Mvc;
using backend.Services.Base;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserJsonService _userJsonService;
        private readonly IUserService _userService;

        public UsersController(IUserJsonService userJsonService, IUserService userService)
        {
            _userJsonService = userJsonService;
            _userService = userService;
        }

        [HttpPost("save-json")]
        public async Task<IActionResult> SaveUserJson([FromBody] object jsonData)
        {
            try
            {

                if (jsonData == null)
                {
                    return BadRequest("Invalid JSON data.");
                }

                var filePath = await _userJsonService.SaveUserJsonAsync(jsonData);



                return Ok(new { FilePath = filePath });
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-json")]
        public async Task<IActionResult> GetUserJson()
        {
            return await _userJsonService.GetUserJsonAsync();
        }


        [HttpGet("user")]
        public async Task<IActionResult> getUser()
        {
            var user =  _userService.getUser() ;
            return Ok(user);
        }
    }
}
