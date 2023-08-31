using Common.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace Common;

public class CallbackMessageEventStrategy : IEventStrategy
{
    private readonly MongoContext _mongoContext;
    public readonly IHubContext<ChatHub> _chatHub;

    public CallbackMessageEventStrategy(MongoContext mongoContext, IHubContext<ChatHub> chatHub)
    {
        _mongoContext = mongoContext;
        _chatHub = chatHub;
    }

    public bool CanHandle(BaseEvent baseEvent)
    {
        return baseEvent is CallbackMessageEvent;
    }

    public async Task Handle(BaseEvent baseEvent)
    {
        var callbackMessage = baseEvent as CallbackMessageEvent;

        var contato = await _mongoContext.Contatos.Find(x => x.ContatoId == callbackMessage.ContatoId).FirstOrDefaultAsync();
        if (contato == null)
        {
            contato = new Contato
            {
                ContatoId = callbackMessage.ContatoId
            };
            await _mongoContext.Contatos.InsertOneAsync(contato);
        }
        await _chatHub.Groups.AddToGroupAsync(contato.ContatoId.ToString(), contato.ContatoId.ToString());

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
            Texto = callbackMessage.CallbackMessage,
            ConversaId = conversa.Id
        };
        await _mongoContext.Mensagens.InsertOneAsync(mensagem);
        await _chatHub.Clients.Group(contato.ContatoId.ToString()).SendAsync("ReceiveMessage", callbackMessage.ContatoId, callbackMessage.CallbackMessage);
    }
}
