using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Messaging.Services;

namespace YoutubeCloneBackend.Services.ConsumeEvents.ConsumeRegistrationEvent
{
    public class ConsumeUserRegistrationEvent : IConsumeUserRegistrationEvent
    {
        private readonly RabbitMQConnectionProvider _rabbitProvider;
        public ConsumeUserRegistrationEvent(RabbitMQConnectionProvider rabbitProvider)
        {
            _rabbitProvider = rabbitProvider;
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
                Console.WriteLine($"Received: {message}");
                await Task.Yield();
            };

            await channel.BasicConsumeAsync(
                    queue: "register_queue",
                    autoAck: true,
                    consumer: consumer
                );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}
