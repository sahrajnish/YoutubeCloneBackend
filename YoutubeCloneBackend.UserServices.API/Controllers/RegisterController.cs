using Microsoft.AspNetCore.Mvc;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Services.User;

namespace YoutubeCloneBackend.UserServices.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;
        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = "Email is required"
                });
            }

            var result = await _userService.InsertUserService(email);
            return Ok(new
            {
                Status = 200,
                result
            });
        }

        [HttpGet("Temp")]
        public IActionResult Temp()
        {
            return Ok(new
            {
                Status = 200,
                Message = "Hello from User Services API"
            });
        }
    }
}
