using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeCloneBackend.Messaging.Services;
using YoutubeCloneBackend.Persistence.RegisterOtp;

namespace YoutubeCloneBackend.Services.ConsumeEvents.ConsumeRegistrationEvent
{
    public class ConsumeUserRegistrationEvent : IConsumeUserRegistrationEvent
    {
        private readonly RabbitMQConnectionProvider _rabbitProvider;
        private readonly IRegisterOtps _registerOtp;
        public ConsumeUserRegistrationEvent(RabbitMQConnectionProvider rabbitProvider, IRegisterOtps registerOtp)
        {
            _rabbitProvider = rabbitProvider;
            _registerOtp = registerOtp;
        }

        public async Task ConsumeEvents(CancellationToken cancellationToken)
        {
            using var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "register_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                // Console.WriteLine($"Received: {message}");

                var payload = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
                if(payload != null && payload.TryGetValue("Email", out var email))
                {
                    var otp = GenerateOtp();
                    var res = await _registerOtp.InsertRegistrationOtpAsync(email, otp);
                    // Console.WriteLine(res?.OtpExpiresAt);
                }

                await Task.Yield();
            };

            await channel.BasicConsumeAsync(
                    queue: "register_queue",
                    autoAck: true,
                    consumer: consumer
                );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        private static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }

}
