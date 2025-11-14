using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;
using YoutubeCloneBackend.Persistence.RegisterOtp;
using YoutubeCloneBackend.Services.SmsEmailService.PublishMailEvents;

namespace YoutubeCloneBackend.Services.SmsEmailService.RegisterOtp
{
    public class GenerateRegisterOtp : IGenerateRegisterOtp
    {
        private readonly IRegisterOtps _registerOtp;
        private readonly IPublishRegisterOtpMailEvent _mailEvent;
        public GenerateRegisterOtp(IRegisterOtps registerOtp, IPublishRegisterOtpMailEvent mailEvent)
        {
            _registerOtp = registerOtp;
            _mailEvent = mailEvent;
        }

        public async Task<RegisterOtpResponseModel> GenerateOtp(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email is required at Generate OTP Service");
            }

            // Generate OTP
            string otp = GenerateOtp();

            // Calls the Persistence layer to Update Temp Table for the user with newly OTP generated.
            // It will receive back OTP metadata from Persistence layer in form of RegisterOtpResponseModel
            var res = await _registerOtp.InsertRegistrationOtpAsync(email, otp);

            if(res == null)
            {
                throw new Exception("Something went wrong while registering OTP");
            }

            // if user on cooldown period
            if(res.ReattemptAfter.HasValue)
            {
                return new RegisterOtpResponseModel
                {
                    OtpExpiresAt = null,
                    RemainingAttempts = res.RemainingAttempts,
                    ReattemptAfter = res.ReattemptAfter
                };
            }

            // Otp generation failed
            if(res.OtpExpiresAt == null)
            {
                throw new Exception("DB did not return OTP expiry — OTP not generated");
            }

            // Publish event for sending OTP to user's email.
            // This will handle emailing OTP asynchronously.
            // Here Publishing Event is done by SmsEmailService and Consuming Event is also done by SmsEmailService to handle emailing Async.
            await _mailEvent.PublishEventToSendRegisterOtp(email, otp);

            // Returns the OTP metadata in form of RegisterOtpResponseModel
            return res;
        }

        private static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
