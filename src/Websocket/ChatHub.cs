using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using Websocket.Models;

namespace Websocket;

public interface IChatHub
{
    Task ReceiveMessage(Guid contatoId, string message);
}

public class ChatHub : Hub<IChatHub>
{
    private readonly MongoContext _mongoContext;

    public ChatHub(MongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task SendMessage(Guid contatoId, string message)
    {
        var contato = await _mongoContext.Contatos.Find(x => x.ContatoId == contatoId).FirstOrDefaultAsync();
        if (contato == null)
        {
            contato = new Contato
            {
                ContatoId = contatoId
            };
            await _mongoContext.Contatos.InsertOneAsync(contato);
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, contato.ContatoId.ToString());

        var conversa = await _mongoContext.Conversas.Find(x => x.ContatoId == contato.Id).FirstOrDefaultAsync();
        if (conversa == null)
        {
            conversa = new Conversa
            {
                ContatoId = contato.Id
            };
            await _mongoContext.Conversas.InsertOneAsync(conversa);
        }
        var mensagem = new Mensagem
        {
            Texto = message,
            ConversaId = conversa.Id
        };
        await _mongoContext.Mensagens.InsertOneAsync(mensagem);
        await Clients.Group(contato.ContatoId.ToString()).ReceiveMessage(contatoId, message);
    }
}
