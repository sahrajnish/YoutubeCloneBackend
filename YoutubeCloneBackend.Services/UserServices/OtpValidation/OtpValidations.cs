using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Persistence.ValidateOtps;
using YoutubeCloneBackend.Services.UserServices.PublishMailEvents;

namespace YoutubeCloneBackend.Services.UserServices.OtpValidation
{
    public class OtpValidations : IOtpValidations
    {
        private readonly IValidateOtp _validateOtp;
        private readonly IPublishMailEvent _mailEvent;
        public OtpValidations(IValidateOtp validateOtp, IPublishMailEvent mailEvent)
        {
            _validateOtp = validateOtp;
            _mailEvent = mailEvent;
        }

        public async Task<VerifyOtpResponseModel?> VerifyRegisterOtpService(string Purpose, string Email, string Otp)
        {
            if(string.IsNullOrEmpty(Purpose))
            {
                throw new ArgumentNullException(nameof(Purpose), "Purpose is required to Verify OTP");
            }

            if(string.IsNullOrWhiteSpace(Email))
            {
                throw new ArgumentNullException(nameof(Email), "Email is required to Verify OTP");
            }

            if(string.IsNullOrEmpty(Otp))
            {
                throw new ArgumentNullException(nameof(Otp), "Please provide OTP to continue.");
            }
            
            if(Purpose.Equals("register", StringComparison.OrdinalIgnoreCase))
            {
                var result = await _validateOtp.VerifyRegisterationOtp(Email, Otp);
                if(result == null)
                {
                    throw new Exception("OTP function returned no data — possible internal error.");
                }

                // Publish Event to Send Welcome Email to User
                await _mailEvent.SendWelcomeEmailToUser(Email);

                return result;
            }

            throw new Exception("Unsuported OTP Purpose.");
        }
    }
}
