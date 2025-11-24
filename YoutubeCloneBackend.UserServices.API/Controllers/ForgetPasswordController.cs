using Microsoft.AspNetCore.Mvc;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Services.UserServices.OtpValidation;
using YoutubeCloneBackend.Services.UserServices.User;

namespace YoutubeCloneBackend.UserServices.API.Controllers
{
    [ApiController]
    [Route("api/User/[controller]")]
    public class ForgetPasswordController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOtpValidations _OtpValidations;
        public ForgetPasswordController(IUserService userService, IOtpValidations otpValidations)
        {
            _userService = userService;
            _OtpValidations = otpValidations;
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

            var result = await _OtpValidations.VerifyOtpService(OtpPurpose.ResetPassword, request.Email, request.Otp);
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
    }
}
