using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Messaging.Services;

namespace YoutubeCloneBackend.Services.SmsEmailService.PublishMailEvents
{
    public class PublishRegisterOtpMailEvent : IPublishRegisterOtpMailEvent
    {
        private readonly RabbitMQConnectionProvider _rabbitProvider;
        public PublishRegisterOtpMailEvent(RabbitMQConnectionProvider connectionProvider)
        {
            _rabbitProvider = connectionProvider;
        }

        public async Task PublishEventToSendRegisterOtp(string email, string otp)
        {
            using var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "register_otp_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var props = new BasicProperties();
            props.Persistent = true;    

            var messageObject = new UserRegisteredEvent
            {
                Email = email,
                Otp = otp
            };
            var messageJson = JsonSerializer.Serialize(messageObject);

            var body = Encoding.UTF8.GetBytes(messageJson);

            await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "register_otp_queue",
                    mandatory: true,
                    basicProperties: props,
                    body: body
                );
        }
    }
}
