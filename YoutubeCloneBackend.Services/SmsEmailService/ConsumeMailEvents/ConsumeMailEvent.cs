using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Messaging.Services;
using YoutubeCloneBackend.Services.SmsEmailService.MailingService.Mails;

namespace YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents
{
    public class ConsumeMailEvent : IConsumeMailEvent
    {
        private readonly RabbitMQConnectionProvider _rabbitProvider;
        private readonly IMail _mail;
        public ConsumeMailEvent(RabbitMQConnectionProvider rabbitProvider, IMail mail)
        {
            _rabbitProvider = rabbitProvider;
            _mail = mail;
        }

        public async Task ConsumeNotifyEvents(CancellationToken cancellationToken)
        {
            using var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "notification_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var payload = JsonSerializer.Deserialize<NotificationEvent>(
                        message,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if(payload != null && !string.IsNullOrWhiteSpace(payload.Email))
                    {
                        var isEmailSent = await _mail.SendNotificationEmail(payload.Purpose, payload.Email);

                        if(!isEmailSent)
                        {
                            await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                            return;
                        }
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                } catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");

                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                    queue: "notification_queue",
                    autoAck: false,
                    consumer: consumer
                );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        public async Task ConsumeOtpEvents(CancellationToken cancellationToken)
        {
            var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "otp_events_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var payload = JsonSerializer.Deserialize<OtpEvent>(
                        message,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (payload == null || payload?.Purpose == null || string.IsNullOrWhiteSpace(payload.Email))
                    {
                        await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                        return;
                    }

                    bool isSent = await _mail.SendOtpEmail(payload.Purpose, payload.Email, payload.Otp);

                    if (!isSent)
                    {
                        await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                        return;
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                } 
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                    queue: "otp_events_queue",
                    autoAck: false,
                    consumer: consumer
                );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}
