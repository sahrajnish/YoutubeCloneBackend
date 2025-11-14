using Microsoft.AspNetCore.Mvc;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Services.UserServices.User;

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

            // Insert User to Temp Table and generate otp
            var otpResult = await _userService.InsertUserToTempTableService(email);

            if(otpResult == null)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "OTP service is unavailable"
                });
            }

            // Check for Cooldown period
            if(otpResult.ReattemptAfter.HasValue)
            {
                return StatusCode(429, new
                {
                    Status = 429,
                    Message = $"Too many attempts. Retry at {otpResult.ReattemptAfter}",
                    ReattemptAt = otpResult.ReattemptAfter,
                    RemainingAttempt = otpResult.RemainingAttempts
                });
            }

            // Check if OTP was created successfully or not.
            if(otpResult.OtpExpiresAt == null)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Failed to generate OTP. Please try again later."
                });
            }

            return Ok(new
            {
                Status = 200,
                Message = "OTP Sent",
                OtpExpireyTime = otpResult.OtpExpiresAt,
                ReattemptAt = otpResult.ReattemptAfter,
                RemainingAttempt = otpResult.RemainingAttempts
            });
        }
    }
}
