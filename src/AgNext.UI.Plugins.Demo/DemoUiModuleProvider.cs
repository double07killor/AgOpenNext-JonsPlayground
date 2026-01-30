namespace AgNext.UI.Plugins.Demo;

using AgNext.UI.ViewModels;

public sealed class DemoUiModuleProvider : IUiModuleProvider
{
    public void RegisterModules(IUiModuleRegistry registry)
    {
        registry.Register(new MetricModuleViewModel(
            "demo-temp",
            "Cab Temp",
            "22.4 C",
            170,
            16,
            140,
            70,
            "#FFB347"));

        registry.Register(new MetricModuleViewModel(
            "demo-downforce",
            "Downforce",
            "115 kg",
            170,
            96,
            140,
            70,
            "#FF6F91"));
    }
}
