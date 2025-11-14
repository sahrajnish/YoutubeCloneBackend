using YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents;

namespace YoutubeCloneBackend.SmsEmailServices.API.Services
{
    // Inherit from BackgroundService
    public class RabbitMqBackgroundService : BackgroundService
    {
        // Create Instance of IConsumeRegisterOtpMailEvent
        private readonly IConsumeRegisterOtpMailEvent _consumeRegisterOtpMailEvent;
        public RabbitMqBackgroundService(IConsumeRegisterOtpMailEvent consumeRegisterOtpMailEvent)
        {
            _consumeRegisterOtpMailEvent = consumeRegisterOtpMailEvent;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // This is a background service that will run till the app runs and it will continuously monitor the register_otp_queue.
            // Call ConsumeEvents method in IConsumeRegisterOtpMailEvent and provide stoppingToken in parameter.
            await _consumeRegisterOtpMailEvent.ConsumeEvents(stoppingToken);
        }
    }
}
