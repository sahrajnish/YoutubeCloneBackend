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

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDTOModel request)
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

            var result = await _userService.VerifyOtpService(OtpPurpose.ResetPassword, request.Email, request.Otp);
            if (result == null)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "OTP validation failed. Please try again."
                });
            }

            return Ok(new
            {
                data = result
            });
        }

        [HttpPost("ResendOtp")]
        public async Task<IActionResult> ResendOtp()
        {
            return Ok();
        }

        [HttpPost("CreatePassword")]
        public async Task<IActionResult> CreatePassword([FromBody] CreatePasswordDTOModel request)
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

            var purpose = NewPasswordTypes.ResetPassword;

            var result = await _userService.CreateNewPasswordService(purpose, request.Email, request.Password, request.ConfirmPassword);

            return Ok(result);
        }
    }
}
