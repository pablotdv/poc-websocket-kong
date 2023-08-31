using System.Runtime.CompilerServices;
using Common;
using Common.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Workerkafka;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConsumer<Null, string> _consumer;
    private readonly List<IEventStrategy> _eventStrategies;    

    public Worker(ILogger<Worker> logger, IConfiguration configuration, MongoContext mongoContext, IHubContext<ChatHub, IChatHub> chatHub)
    {
        _logger = logger;
        var config = new ConsumerConfig
        {
            BootstrapServers = configuration?.GetConnectionString("Kafka") ?? "localhost:9092",
            GroupId = "poc.consumer",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _consumer.Subscribe("poc.topic");

        _eventStrategies = new List<IEventStrategy>()
        {
            new MessageEventStrategy(),
            new CallbackMessageEventStrategy(mongoContext, chatHub),
            new CallbackStatusEventStrategy(),
            new TransbordoEventStrategy()
        };

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var consumeResult = _consumer.Consume(stoppingToken);
           
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            var baseEvent = JsonConvert.DeserializeObject<BaseEvent>(consumeResult.Message.Value, jsonSerializerSettings);

            var eventStrategy = _eventStrategies.FirstOrDefault(es => es.CanHandle(baseEvent));

            if (eventStrategy != null)
            {
                eventStrategy.Handle(baseEvent);
            }
            else
            {
                Console.WriteLine("No strategy found");
            }

            await Task.CompletedTask;
        }
    }
}

