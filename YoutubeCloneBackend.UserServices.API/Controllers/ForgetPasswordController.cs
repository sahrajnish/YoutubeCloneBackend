using Microsoft.AspNetCore.Mvc;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Services.UserServices.User;

namespace YoutubeCloneBackend.UserServices.API.Controllers
{
    [ApiController]
    [Route("api/User/[controller]")]
    public class ForgetPasswordController : Controller
    {
        private readonly IUserService _userService;
        public ForgetPasswordController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromBody] OtpDTOModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = "Invalid Input",
                    Errors = ModelState
                });
            }

            var result = await _userService.SendResetOtpService(request.Email);

            return Ok(result);
        }
    }
}
