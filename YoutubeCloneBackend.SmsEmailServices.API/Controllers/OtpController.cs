using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;
using YoutubeCloneBackend.Services.SmsEmailService.RegisterOtp;

namespace YoutubeCloneBackend.SmsEmailServices.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OtpController : ControllerBase
    {
        private readonly IGenerateRegisterOtp _generateOtp;
        public OtpController(IGenerateRegisterOtp generateOtp)
        {
            _generateOtp = generateOtp;
        }

        // Receives the Request from UserService on Path "/api/Otp/Send" with email as JSON.
        [HttpPost("Send")]
        public async Task<IActionResult> Otp([FromBody] RegisterOtpRequestDto request)
        {
            if(request == null || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new
                {
                    Message = "Email is required for OTP generation"
                });
            }

            // It will call IGenerateRegisterOtp to generate OTP and receives back the OTP metadata.
            var response = await _generateOtp.GenerateOtp(request.Email);
            if(response == null)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Something went wrong while generating OTP"
                });
            }

            // Sends the OTP metadata back to UserService which called this controller.
            return Ok(response);
        }
    }
}
