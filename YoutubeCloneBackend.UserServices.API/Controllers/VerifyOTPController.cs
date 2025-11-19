using Microsoft.AspNetCore.Mvc;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Services.UserServices.OtpValidation;

namespace YoutubeCloneBackend.UserServices.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerifyOTPController : Controller
    {
        private readonly IOtpValidations _OtpValidations;
        public VerifyOTPController(IOtpValidations otpValidations)
        {
            _OtpValidations = otpValidations;
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOtpDTOModel request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = "Invalid Input",
                    Errors = ModelState
                });
            }

            var result = await _OtpValidations.VerifyRegisterOtpService(request.Purpose, request.Email, request.Otp);
            if(result == null)
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
