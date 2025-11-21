using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Messaging.Services;

namespace YoutubeCloneBackend.Services.UserServices.PublishMailEvents
{
    public class PublishMailEvent : IPublishMailEvent
    {
        private readonly RabbitMQConnectionProvider _rabbitProvider;
        public PublishMailEvent(RabbitMQConnectionProvider rabbitProvider)
        {
            _rabbitProvider = rabbitProvider;
        }

        public async Task SendOtpToUser(string purpose, string email, string otp)
        {
            using var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "otp_events_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var props = new BasicProperties();
            props.Persistent = true;

            var messageObject = new OtpEvent
            {
                Purpose = purpose,
                Email = email,
                Otp = otp,
            };

            var messageJson = JsonSerializer.Serialize(messageObject);

            var body = Encoding.UTF8.GetBytes(messageJson);

            await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "otp_events_queue",
                    mandatory: true,
                    basicProperties: props,
                    body: body
                );
        }

        public async Task SendWelcomeEmailToUser(string email)
        {
            using var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "new_users_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var props = new BasicProperties();
            props.Persistent = true;

            var messageObject = new
            {
                Email = email
            };

            var messageJson = JsonSerializer.Serialize(messageObject);

            var body = Encoding.UTF8.GetBytes(messageJson);

            await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "new_users_queue",
                    mandatory: true,
                    basicProperties: props,
                    body: body
                );
        }
    }
}
