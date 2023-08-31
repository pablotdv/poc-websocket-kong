namespace Common;

public interface IEventStrategy
{
    bool CanHandle(BaseEvent baseEvent);
    Task Handle(BaseEvent baseEvent);
}
