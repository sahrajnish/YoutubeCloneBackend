using Microsoft.AspNetCore.Mvc;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Services.UserServices.OtpValidation;
using YoutubeCloneBackend.Services.UserServices.User;

namespace YoutubeCloneBackend.UserServices.API.Controllers
{
    [ApiController]
    [Route("/api/User/[controller]")]
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOtpValidations _OtpValidations;
        public RegisterController(IUserService userService, IOtpValidations otpValidations)
        {
            _userService = userService;
            _OtpValidations = otpValidations;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDTO request)
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

            // Insert User to Temp Table and generate otp
            var otpResult = await _userService.InsertUserToTempTableService(request.Email);
            if(otpResult == null)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "OTP service is unavailable"
                });
            }

            return Ok(new
            {
                data = otpResult
            });
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

            var result = await _OtpValidations.VerifyOtpService(OtpPurpose.Register, request.Email, request.Otp);
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

            var purpose = NewPasswordTypes.NewPassword;

            var result = await _userService.CreateNewPasswordService(purpose, request.Email, request.Password, request.ConfirmPassword);

            return Ok(result);
        }
    }
}
