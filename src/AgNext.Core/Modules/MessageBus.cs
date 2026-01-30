namespace AgNext.Core.Modules;

using System.Collections.Concurrent;

public sealed class MessageBus : IMessageBus
{
    private sealed record Subscription(Action<object> Handler, int Priority, IMessageQueue? Queue);

    private readonly ConcurrentDictionary<Type, List<Subscription>> _subscriptions = new();
    private readonly ConcurrentDictionary<Type, object> _lastMessages = new();

    public IDisposable Subscribe<T>(Action<T> handler) => Subscribe(handler, priority: 0);

    public IDisposable Subscribe<T>(Action<T> handler, int priority)
    {
        return AddSubscription(typeof(T), o => handler((T)o), priority, queue: null);
    }

    public IDisposable SubscribeQueued<T>(Action<T> handler, IMessageQueue queue)
    {
        return AddSubscription(typeof(T), o => handler((T)o), priority: 0, queue);
    }

    public void Publish<T>(in T message)
    {
        _lastMessages[typeof(T)] = message!;
        if (!_subscriptions.TryGetValue(typeof(T), out var list))
        {
            return;
        }

        foreach (var subscription in list.OrderByDescending(s => s.Priority))
        {
            if (subscription.Queue != null)
            {
                var captured = message;
                subscription.Queue.Enqueue(() => subscription.Handler(captured!));
            }
            else
            {
                subscription.Handler(message!);
            }
        }
    }

    public bool TryGetLastMessage<T>(out T message)
    {
        if (_lastMessages.TryGetValue(typeof(T), out var value) && value is T typed)
        {
            message = typed;
            return true;
        }

        message = default!;
        return false;
    }

    private IDisposable AddSubscription(Type type, Action<object> handler, int priority, IMessageQueue? queue)
    {
        var subscription = new Subscription(handler, priority, queue);
        var list = _subscriptions.GetOrAdd(type, _ => new List<Subscription>());
        lock (list)
        {
            list.Add(subscription);
        }

        return new DisposableAction(() =>
        {
            lock (list)
            {
                list.Remove(subscription);
            }
        });
    }

    private sealed class DisposableAction : IDisposable
    {
        private readonly Action _dispose;
        private bool _isDisposed;

        public DisposableAction(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _dispose();
            _isDisposed = true;
        }
    }
}
