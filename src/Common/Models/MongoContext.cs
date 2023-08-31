using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Common.Models;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("DefaultConnection"));
        _database = client.GetDatabase("Chat");
    }

    public IMongoCollection<Contato> Contatos
    {
        get
        {
            return _database.GetCollection<Contato>("Contatos");
        }
    }

    public IMongoCollection<Conversa> Conversas
    {
        get
        {
            return _database.GetCollection<Conversa>("Conversas");
        }
    }

    public IMongoCollection<Mensagem> Mensagens
    {
        get
        {
            return _database.GetCollection<Mensagem>("Mensagens");
        }
    }
}