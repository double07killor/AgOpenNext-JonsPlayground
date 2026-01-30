namespace AgNext.UI.ViewModels;

public interface IUiModuleRegistry
{
    void Register(UiModuleViewModel module);
}

public sealed class UiModuleRegistry : IUiModuleRegistry
{
    private readonly Action<UiModuleViewModel> _register;

    public UiModuleRegistry(Action<UiModuleViewModel> register)
    {
        _register = register;
    }

    public void Register(UiModuleViewModel module)
    {
        _register(module);
    }
}
