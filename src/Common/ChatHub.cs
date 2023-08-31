using Common.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Common;

public interface IChatHub
{
    Task ReceiveMessage(Guid contatoId, string message);
    Task SendMessage(Guid contatoId, string message);
}

public class ChatHub : Hub<IChatHub>
{
    private readonly MongoContext _mongoContext;
    private readonly IProducer<Null, String> _producer;

    public ChatHub(MongoContext mongoContext, IProducer<Null, string> producer)
    {
        _mongoContext = mongoContext;
        _producer = producer;
    }

    public async Task SendMessage(Guid contatoId, string message)
    {
        var callbackMessageEvent = new CallbackMessageEvent()
        {
            Id = Guid.NewGuid(),
            CallbackMessage = message,
            ContatoId = contatoId,
            Timestamp = DateTime.Now,
            ConnectionId = Context.ConnectionId
        };

        var jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        var jsonSerializer = JsonConvert.SerializeObject(callbackMessageEvent, jsonSerializerSettings);


        var result = await _producer.ProduceAsync("poc.topic", new Message<Null, string> { Value = jsonSerializer });

    }
}
