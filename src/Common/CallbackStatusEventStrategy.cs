namespace Common;

public class CallbackStatusEventStrategy : IEventStrategy
{
    public bool CanHandle(BaseEvent baseEvent)
    {
        return baseEvent is CallbackStatusEvent;
    }

    public Task Handle(BaseEvent baseEvent)
    {
        var callbackStatus = baseEvent as CallbackStatusEvent;

        Console.WriteLine("CallbackStatusEvent");
        Console.WriteLine($"Id: {callbackStatus.Id}");
        Console.WriteLine($"CallbackStatus: {callbackStatus.CallbackStatus}");
        Console.WriteLine($"Timestamp: {callbackStatus.Timestamp}");
        return Task.CompletedTask;
    }
}
