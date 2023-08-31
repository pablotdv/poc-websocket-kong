namespace Common;

public class TransbordoEventStrategy : IEventStrategy
{
    public bool CanHandle(BaseEvent baseEvent)
    {
        return baseEvent is TransbordoEvent;
    }

    public Task Handle(BaseEvent baseEvent)
    {
        var transbordo = baseEvent as TransbordoEvent;

        Console.WriteLine("TransbordoEvent");
        Console.WriteLine($"Id: {transbordo.Id}");
        Console.WriteLine($"Transbordo: {transbordo.Transbordo}");
        Console.WriteLine($"Timestamp: {transbordo.Timestamp}");
        return Task.CompletedTask;
    }
}
