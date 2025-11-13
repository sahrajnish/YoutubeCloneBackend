using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.Mailjet;

namespace YoutubeCloneBackend.Services.MailingService.MailingOtps
{
    public class MailOtp : IMailOtp
    {
        private readonly MailjetOptions _options;
        public MailOtp(IOptions<MailjetOptions> options)
        {
            _options = options.Value;
        }

        public async Task<bool> SendRegisterOtp(string email, string otp, DateTime? otpExpiresAt)
        {
            var client = new MailjetClient(_options.ApiKey, _options.ApiSecret);

            var htmlbody = $@"
                <div style='font-family:Arial;padding:20px'>
                    <h2>Your OTP Code</h2>
                    <p>Your verification OTP is:</p>
                    <h1 style='color:#007bff;'>{otp}</h1>
                    <p>It will expire in 10 minutes.</p>
                </div>
            ";

            var request = new MailjetRequest
            {
                Resource = SendV31.Resource
            }
            .Property(Send.Messages, new JArray
            {
                new JObject
                {
                    {"From", new JObject
                    {
                        {"Email", _options.FromEmail },
                        {"Name", _options.FromName }
                    } },
                    {"To", new JArray
                    {
                        new JObject
                        {
                            {"Email", email }
                        }
                    } },
                    {"Subject", "Your OTP Code" },
                    {"HTMLPart", htmlbody }
                }
            });

            var response = await client.PostAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
