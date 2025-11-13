using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeCloneBackend.Messaging.Services;

namespace YoutubeCloneBackend.Services.PublishEvents
{
    public class PublishRegisteredUserEvent : IPublishRegisteredUserEvent
    {
        private readonly RabbitMQConnectionProvider _rabbitProvider;
        public PublishRegisteredUserEvent(RabbitMQConnectionProvider rabbitProvider)
        {
            _rabbitProvider = rabbitProvider;
        }

        public async Task PublishRegisteredUserEventService(string email)
        {

            using var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "register_queue",
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
                    routingKey: "register_queue",
                    mandatory: true,
                    basicProperties: props,
                    body: body
                );

            Console.WriteLine($"Message published for email: {messageJson}");
        }
    }
}
