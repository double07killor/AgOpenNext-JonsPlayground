namespace AgNext.Core.Modules;

public interface IMessageBus
{
    IDisposable Subscribe<T>(Action<T> handler);
    IDisposable Subscribe<T>(Action<T> handler, int priority);
    IDisposable SubscribeQueued<T>(Action<T> handler, IMessageQueue queue);
    void Publish<T>(in T message);
    bool TryGetLastMessage<T>(out T message);
}

public interface IMessageQueue
{
    void Enqueue(Action action);
    void ProcessQueue();
    int QueuedCount { get; }
    void Clear();
}
