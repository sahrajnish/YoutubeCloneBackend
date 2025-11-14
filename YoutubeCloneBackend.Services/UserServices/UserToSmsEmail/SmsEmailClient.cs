using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;

namespace YoutubeCloneBackend.Services.UserServices.UserToSmsEmail
{
    public class SmsEmailClient : ISmsEmailClient
    {
        private readonly HttpClient _httpClient;
        public SmsEmailClient(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("SmsEmailService");
        }

        public async Task<RegisterOtpResponseModel?> SendOtpAsync(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email is required at SendOtpAsync method");
            }

            // Connect Synchronously To SmsEmailService API provided with path and email to generate OTP.
            // Connecting Synchronously here does not create SPoF as it is required to receive the OTP metadata at the same time of registeration
            // Hence we will not use RabbitMQ here to connect Asynchronously to SmsEmailService.
            var response = await _httpClient.PostAsJsonAsync("api/Otp/Send", new { Email = email });

            if(!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get OTP. SmsEmailService responded {content}");
            }

            // Read the response received from SmsEmailService which is in JSON and convert it to RegisterOtpResponseModel which is required by User Service.
            return await response.Content.ReadFromJsonAsync<RegisterOtpResponseModel>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
