using YoutubeCloneBackend.Services.SmsEmailService.ConsumeMailEvents;

namespace YoutubeCloneBackend.SmsEmailServices.API.Services
{
    // Inherit from BackgroundService
    public class RabbitMqBackgroundService : BackgroundService
    {
        // Create Instance of IConsumeRegisterOtpMailEvent
        private readonly IConsumeMailEvent _consumeMailEvent;
        public RabbitMqBackgroundService(IConsumeMailEvent consumeMailEvent)
        {
            _consumeMailEvent = consumeMailEvent;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // This is a background service that will run till the app runs and it will continuously monitor the register_otp_queue.
            // Call ConsumeEvents method in IConsumeRegisterOtpMailEvent and provide stoppingToken in parameter.
            var tasks = new List<Task>
            {
                Task.Run(() => _consumeMailEvent.ConsumeRegisterOtpEvents(stoppingToken), stoppingToken),
                Task.Run(() => _consumeMailEvent.ConsumeNewUserEvents(stoppingToken), stoppingToken)
            };
            

            return Task.WhenAll(tasks);
        }
    }
}
