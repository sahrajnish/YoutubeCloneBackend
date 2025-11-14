using YoutubeCloneBackend.Messaging.Services;
using YoutubeCloneBackend.Services.RegisterServices;
using YoutubeCloneBackend.UserServices.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterUserDIServices();

// Connect to SmsEmailService with baseUrl in appsettings.json.
// Also configure the client header to provide ApiKey from appsettings to get access of SmsEmailService, else request will be declined.
builder.Services.AddHttpClient("SmsEmailService", client =>
{
    string baseUrl = builder.Configuration["SmsEmailService:BaseUrl"];
    if(string.IsNullOrEmpty(baseUrl))
    {
        throw new Exception("SmsEmailService BaseUrl is missing in appsettings.json");
    }
    client.BaseAddress = new Uri(baseUrl);
})
.ConfigureHttpClient((sp, client) =>
{
    // Add ApiKey to header for getting access to SmsEmailService.
    var config = sp.GetRequiredService<IConfiguration>();
    var apiKey = config["InternalAuth:ApiKey"];

    client.DefaultRequestHeaders.Add("X-Internal-Api-Key", apiKey);
});

var rabbitProvider = new RabbitMQConnectionProvider();
await rabbitProvider.InitializeAsync();
builder.Services.AddSingleton(rabbitProvider);

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

app.MapControllers();

app.Run();
