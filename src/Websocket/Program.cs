using Common;
using Common.Models;
using Confluent.Kafka;
using Websocket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis") ?? "");
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton(c =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = builder.Configuration.GetConnectionString("Kafka") ?? ""
    };
    return new ProducerBuilder<Null, string>(config).Build();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
