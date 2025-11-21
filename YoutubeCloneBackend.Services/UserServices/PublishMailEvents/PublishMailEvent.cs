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

        public async Task SendOtpToUser(OtpEvent eventDetails)
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

            var messageJson = JsonSerializer.Serialize(eventDetails);

            var body = Encoding.UTF8.GetBytes(messageJson);

            await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "otp_events_queue",
                    mandatory: true,
                    basicProperties: props,
                    body: body
                );
        }

        public async Task NotifyUser(NotificationEvent eventDetails)
        {
            using var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "notification_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var props = new BasicProperties();
            props.Persistent = true;

            var messageJson = JsonSerializer.Serialize(eventDetails);

            var body = Encoding.UTF8.GetBytes(messageJson);

            await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "notification_queue",
                    mandatory: true,
                    basicProperties: props,
                    body: body
                );
        }
    }
}
