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
using YoutubeCloneBackend.Services.SmsEmailService.MailingService.Mails;
using static System.Net.WebRequestMethods;

namespace YoutubeCloneBackend.Services.SmsEmailService.MailingService.Mails
{
    public class Mail : IMail
    {
        private readonly MailjetOptions _options;
        public Mail(IOptions<MailjetOptions> options)
        {
            _options = options.Value;
        }

        public async Task<bool> SendOtpEmail(string purpose, string email, string otp)
        {
            var client = new MailjetClient(_options.ApiKey, _options.ApiSecret);

            string subject = purpose.ToLower() switch
            {
                "register" => "Your Registration OTP Code",
                "reset" => "Your Password Reset OTP",
                _ => "Your OTP Code"
            };

            string description = purpose.ToLower() switch
            {
                "register" => "Use this OTP to complete your registration.",
                "reset" => "Use this OTP to reset your password.",
                _ => "Use this OTP for verification."
            };

            var htmlbody = $@"
                    <div style='font-family:Arial;padding:20px'>
                        <h2>{subject}</h2>
                        <h1 style='color:#007bff;'>{otp}</h1>
                        <p>{description}</p>
                        <p>This OTP expires in <strong>10 minutes</strong>.</p>
                    </div>";

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
                    {"Subject", subject },
                    {"HTMLPart", htmlbody }
                }
            });

            var response = await client.PostAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SendWelcomeEmail(string email)
        {
            var client = new MailjetClient(_options.ApiKey, _options.ApiSecret);

            var htmlBody = $@"
                <div style='font-family:Arial;padding:20px;line-height:1.6'>
                    <h2 style='color:#007bff;'>Welcome to Matter of News!</h2>

                    <p>Hi there,</p>

                    <p>
                        Your email has been successfully verified. We're excited to have you onboard!
                        You now have full access to your Matter of News account.
                    </p>

                    <p>
                        Stay tuned for the latest curated news and personalized updates.
                    </p>

                    <p style='margin-top:25px;'>Enjoy exploring,</p>
                    <p><strong>The Matter of News Team</strong></p>
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
                    {"Subject", "Welcome to Matter of News Family!" },
                    {"HTMLPart", htmlBody }
                }
            });

            var response = await client.PostAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
