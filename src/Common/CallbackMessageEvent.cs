namespace Common;

public class CallbackMessageEvent : BaseEvent
{
    public string CallbackMessage { get; set; }
    public Guid ContatoId { get; set; }
    public string ConnectionId { get; set; }
}
