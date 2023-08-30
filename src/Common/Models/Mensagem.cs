using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Models;

public class Mensagem
{
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Texto { get; set; }

    public required Guid ConversaId { get; set; }
}
