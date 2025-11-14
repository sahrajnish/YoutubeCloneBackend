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
using YoutubeCloneBackend.Services.SmsEmailService.MailingService.MailingOtps;

namespace YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents
{
    public class ConsumeRegisterOtpMailEvent : IConsumeRegisterOtpMailEvent
    {
        private readonly RabbitMQConnectionProvider _rabbitProvider;
        private readonly IMailOtp _mailOtp;
        public ConsumeRegisterOtpMailEvent(RabbitMQConnectionProvider rabbitProvider, IMailOtp mailOtp)
        {
            _rabbitProvider = rabbitProvider;
            _mailOtp = mailOtp;
        }

        public async Task ConsumeEvents(CancellationToken cancellationToken)
        {
            using var channel = await _rabbitProvider.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: "register_otp_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

            var consumer = new AsyncEventingBasicConsumer(channel);

            // Consume the events from register_otp_async
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    // Console.WriteLine($"Received Message: {message}");

                    var payload = JsonSerializer.Deserialize<UserRegisteredEvent>(
                        message,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (payload != null && !string.IsNullOrWhiteSpace(payload.Email))
                    {
                        // Calls the IMailOtp in MailingService under SmsEmailService to send OTP to user's email.
                        var isOtpSent = await _mailOtp.SendRegisterOtp(payload.Email, payload.Otp);

                        if (!isOtpSent)
                        {
                            // Message sent failed. So Nack this and then requeue it.
                            await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                            return;
                        }
                    }

                    // Ack here, means email sent successfully. Now delete from queue.
                    // Here multipe: false means Donot Ack messeges upto the provided delivery tag.
                    // if multiple: true 
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                } catch (Exception e)
                {
                    Console.WriteLine($"Error processing message: {e.Message}");

                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };

            // Auto Acknowledge is set to false.
            // Means RabbitMQ automatically doesnot mark true for each messages that it checks from queue.
            await channel.BasicConsumeAsync(
                    queue: "register_otp_queue",
                    autoAck: false,
                    consumer: consumer
                );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}
