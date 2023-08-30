using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Websocket.Models;

public class Conversa
{
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();

    public required Guid ContatoId { get; set; }
}
