using YoutubeCloneBackend.Core.Mailjet;
using YoutubeCloneBackend.Messaging.Services;
using YoutubeCloneBackend.Services.RegisterServices;
using YoutubeCloneBackend.SmsEmailServices.API.Middlewares;
using YoutubeCloneBackend.SmsEmailServices.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mailjet configuration
builder.Services.Configure<MailjetOptions>(
        builder.Configuration.GetSection("Mailjet")
);

// connecting to rabbit mq -- YoutubeCloneBackend.Messaging.Services.RabbitMQConnectionProvider.cs
var rabbitProvider = new RabbitMQConnectionProvider();
await rabbitProvider.InitializeAsync();
builder.Services.AddSingleton(rabbitProvider);

// Register Dependency Injection -- YoutubeCloneBackend.Services.RegisterServices.SmsEmailServiceRegistry.cs;
builder.Services.RegisterSmsEmailDIServices();

// Add Background rabbit mq queue check in infinite loop
builder.Services.AddHostedService<RabbitMqBackgroundService>();

/*
    Flow
    rabbit mq connection ---> YoutubeCloneBackend.Messaging ---> RegisterSmsEmailDIServices ---> 
    YoutubeCloneBackend.Services.RegisterServices ---> AddHostedService ---> RabbitMqBackgroundService ---> 
    IConsumeUserRegistrationEvent ---> YoutubeCloneBackend.Services.ConsumeEvents ---> IConsumeUserRegistrationEvent.ConsumeEvents
 */

var app = builder.Build();
app.UseMiddleware<ExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Allow only authorized services to access SmsEmailService. Services with APIKey in their headers will be allowed to access this microservice.
app.Use(async (context, next) =>
{
    var config = context.RequestServices.GetRequiredService<IConfiguration>();
    var expectedKey = config["InternalAuth:ApiKey"];

    if(!context.Request.Headers.TryGetValue("X-Internal-Api-Key", out var providedKey) || providedKey !=  expectedKey)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized internal request");
        return; 
    }

    await next();
});

app.MapControllers();

app.Run();
