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
using YoutubeCloneBackend.Core.User;
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

        public async Task<bool> SendOtpEmail(OtpPurpose purpose, string email, string otp)
        {
            var client = new MailjetClient(_options.ApiKey, _options.ApiSecret);

            string subject = purpose switch
            {
                OtpPurpose.Register => "Your Registration OTP Code",
                OtpPurpose.ResetPassword => "Your Password Reset OTP",
                _ => "Your OTP Code"
            };

            string description = purpose switch
            {
                OtpPurpose.Register => "Use this OTP to complete your registration.",
                OtpPurpose.ResetPassword => "Use this OTP to reset your password.",
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

        public async Task<bool> SendNotificationEmail(NotificationPurpose purpose, string email)
        {
            var client = new MailjetClient(_options.ApiKey, _options.ApiSecret);

            string subject = purpose switch
            {
                NotificationPurpose.WelcomeUser => "Welcome to Matter of News!",
                NotificationPurpose.PasswordCreated => "Your Password Has Been Created",
                NotificationPurpose.PasswordReset => "Your Password Has Been Reset Successfully",
                NotificationPurpose.PasswordChanged => "Your Password Was Changed",
                _ => "Account Notification"
            };

            string description = purpose switch
            {
                NotificationPurpose.WelcomeUser => "Your email has been verified successfully. Welcome aboard!",
                NotificationPurpose.PasswordCreated => "Your new password has been successfully created.",
                NotificationPurpose.PasswordReset => "Your password has been reset successfully.",
                NotificationPurpose.PasswordChanged => "Your password was recently changed.",
                _ => "Here is an update regarding your account."
            };

            var htmlBody = $@"
                    <div style='font-family:Arial;padding:20px;line-height:1.6;color:#333;'>
                        <h2 style='color:#007bff;'>{subject}</h2>

                        <p>Hi there,</p>

                        <p>{description}</p>

                        {(purpose == NotificationPurpose.WelcomeUser
                            ? "<p>We're excited to have you join the Matter of News community. Stay tuned for curated updates and personalized content.</p>"
                            : "<p>If you did not make this request, please secure your account immediately.</p>"
                        )}

                        <p style='margin-top:25px;'>Regards,</p>
                        <p><strong>The Matter of News Team</strong></p>

                        <hr style='margin-top:30px;border:0;border-top:1px solid #ddd;'/>
                        <p style='font-size:12px;color:#777;'>
                            This is an automated message from Matter of News.
                        </p>
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
                    {"HTMLPart", htmlBody }
                }
            });

            var response = await client.PostAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
