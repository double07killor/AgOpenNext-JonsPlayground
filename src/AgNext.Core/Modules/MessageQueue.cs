namespace AgNext.Core.Modules;

public sealed class MessageQueue : IMessageQueue
{
    private readonly Queue<Action> _queue = new();

    public int QueuedCount => _queue.Count;

    public void Enqueue(Action action)
    {
        _queue.Enqueue(action);
    }

    public void ProcessQueue()
    {
        while (_queue.Count > 0)
        {
            var action = _queue.Dequeue();
            action();
        }
    }

    public void Clear()
    {
        _queue.Clear();
    }
}
