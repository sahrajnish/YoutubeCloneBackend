using YoutubeCloneBackend.Services.ConsumeEvents.ConsumeRegistrationEvent;

namespace YoutubeCloneBackend.SmsEmailServices.API.Services
{
    // Inherit from BackgroundService
    public class RabbitMqBackgroundService : BackgroundService
    {
        // Create Instance of IConsumeUserRegistrationEvent
        private readonly IConsumeUserRegistrationEvent _consumeUserRegistrationEvent;
        public RabbitMqBackgroundService(IConsumeUserRegistrationEvent consumeUserRegistrationEvent)
        {
            _consumeUserRegistrationEvent = consumeUserRegistrationEvent;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Call ConsumeEvents method in IConsumeUserRegistrationEvent and provide stoppingToken in parameter.
            await _consumeUserRegistrationEvent.ConsumeEvents(stoppingToken);
        }
    }
}
