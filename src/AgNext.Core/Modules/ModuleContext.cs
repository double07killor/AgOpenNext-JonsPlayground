namespace AgNext.Core.Modules;

public interface IModuleContext
{
    IMessageBus MessageBus { get; }
    IServiceProvider Services { get; }
    IMessageQueue CreateMessageQueue();
}

public sealed class ModuleContext : IModuleContext
{
    public ModuleContext(IMessageBus bus, IServiceProvider services)
    {
        MessageBus = bus;
        Services = services;
    }

    public IMessageBus MessageBus { get; }
    public IServiceProvider Services { get; }

    public IMessageQueue CreateMessageQueue() => new MessageQueue();
}
