using Microsoft.AspNetCore.Mvc;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Services.UserServices.User;

namespace YoutubeCloneBackend.UserServices.API.Controllers
{
    [ApiController]
    [Route("api/User/[controller]")]
    public class CreatePasswordController : Controller
    {
        private readonly IUserService _userService;
        public CreatePasswordController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] CreatePasswordDTOModel request)
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

            var result = await _userService.CreateNewPasswordService(request.Email, request.Password, request.ConfirmPassword);

            return Ok(result);
        }
    }
}
