using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using Common;
using Common.Models;

namespace Webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class ConversasController : ControllerBase
{
    public readonly IHubContext<ChatHub> _chatHub;

    public ConversasController(IHubContext<ChatHub> chatHub)
    {
        _chatHub = chatHub;
    }


    [HttpPost("{conversaId}/mensagens")]
    public async Task<IActionResult> SendMessage([FromServices] MongoContext mongoContext, Guid conversaId, string message)
    {
        var conversa = await mongoContext.Conversas.Find(x => x.Id == conversaId).FirstOrDefaultAsync();
        if (conversa == null)
        {
            return NotFound();
        }
        var contato = await mongoContext.Contatos.Find(x => x.Id == conversa.ContatoId).FirstOrDefaultAsync();

        await _chatHub.Clients.Group(contato.ContatoId.ToString()).SendAsync("ReceiveMessage", "API Says", message);

        return Ok();
    }
}
