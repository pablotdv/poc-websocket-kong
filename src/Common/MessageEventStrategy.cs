namespace Common;

public class MessageEventStrategy : IEventStrategy
{
    public bool CanHandle(BaseEvent baseEvent)
    {
        return baseEvent is MessageEvent;
    }

    public Task Handle(BaseEvent baseEvent)
    {
        var message = baseEvent as MessageEvent;

        Console.WriteLine("MessageEvent");
        Console.WriteLine($"Id: {message.Id}");
        Console.WriteLine($"Message: {message.Message}");
        Console.WriteLine($"Timestamp: {message.Timestamp}");
        return Task.CompletedTask;
    }
}
