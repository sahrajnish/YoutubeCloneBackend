using YoutubeCloneBackend.Messaging.Services;
using YoutubeCloneBackend.Services.RegisterServices;
using YoutubeCloneBackend.UserServices.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDIServices();

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
