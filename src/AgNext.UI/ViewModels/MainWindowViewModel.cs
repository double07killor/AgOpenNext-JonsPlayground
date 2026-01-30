namespace AgNext.UI.ViewModels;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Avalonia.Threading;
using AgNext.Core.Messages;
using AgNext.Core.Modules;
using AgNext.Core.Runtime;
using AgNext.Core.Simulation;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IMessageBus _bus;
    private readonly ModuleHost _host;
    private readonly CoreRuntime _runtime;
    private readonly DispatcherTimer _timer;
    private SimulationSnapshot _snapshot;
    private UiState _uiState = new(new SimulationSnapshot(), new SectionStateMessage(Array.Empty<bool>()), new RateCommandMessage(Array.Empty<double>()), new MonitoringState(0, 0, "None", 0, 0), new PlanterMonitoringState(Array.Empty<double>()));
    private readonly double _stepSeconds;
    private bool _isUnlocked;
    private bool _isBoundaryEditMode;
    private readonly SectionStatusModuleViewModel _sectionsModule;
    private readonly UiLayoutStore _layoutStore;

    public ObservableCollection<UiModuleViewModel> OverlayModules { get; } = new();
    public ObservableCollection<UiModuleViewModel> RightRailModules { get; } = new();
    public ObservableCollection<UiModuleViewModel> StatusModules { get; } = new();

    private double _targetSpeedMps;
    private double _steerInputRad;

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainWindowViewModel()
    {
        _snapshot = new SimulationSnapshot();
        _bus = new MessageBus();
        _host = new ModuleHost(_bus, new SimpleServiceProvider());
        var simulator = SimulatorFactory.BuildDefault();
        _stepSeconds = simulator.StepSeconds;
        var transport = new AgNext.Hardware.Aio.InMemoryAioTransport();
        _host.Register(new AgNext.Hardware.Aio.AioBridgeModule(transport));
        _host.Register(new AgNext.Hardware.Aio.AioSimulationModule(transport));
        _host.Register(new SimulationModule(simulator, publishSensors: false));
        _host.Register(new AgNext.Core.Plugins.KinematicsModule());
        _host.Register(new AgNext.Core.Plugins.GuidanceModule(simulator.Boundary, simulator.SwathHeadingRad, simulator.SwathSpacingMeters));
        _host.Register(new AgNext.Core.Plugins.SectionControlModule());
        _host.Register(new AgNext.Core.Plugins.RateControlModule());
        _host.Register(new AgNext.Core.Plugins.MonitoringModule());
        _host.Register(new AgNext.Core.Plugins.PlanterMonitoringModule());
        _host.Register(new AgNext.Core.Plugins.UiBridgeModule());

        _host.InitializeAsync().GetAwaiter().GetResult();
        _host.StartAsync().GetAwaiter().GetResult();

        _layoutStore = new UiLayoutStore();

        LoadPluginAssemblies();

        var registry = new UiModuleRegistry(RegisterModule);
        RegisterBuiltinModules(registry);
        RegisterPluginModules(registry);

        _sectionsModule = new SectionStatusModuleViewModel("sections", "Sections");
        RegisterModule(_sectionsModule);
        var statusBarModule = new StatusBarModuleViewModel("status-demo", "Demo: 6-row planter, AB swaths, autosteer preview, section lookahead");
        RegisterModule(statusBarModule);

        for (var i = 0; i < 6; i++)
        {
            _sectionsModule.Rows.Add(new RowStatusViewModel(i + 1));
        }

        ApplyLayout(_layoutStore.Load());

        _bus.Subscribe<UiState>(state =>
        {
            UiState = state;
            Snapshot = state.Snapshot;
            UpdateMetricModules();
            UpdateRows();
        });

        _runtime = new CoreRuntime(new SimClock(_stepSeconds), _host);
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(_stepSeconds * 1000) };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();

        _runtime.Tick();
    }

    public SimulationSnapshot Snapshot
    {
        get => _snapshot;
        private set
        {
            _snapshot = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SpeedKph));
            OnPropertyChanged(nameof(HeadingDeg));
            OnPropertyChanged(nameof(AutoSteerStatus));
        }
    }

    public string SpeedKph => (Snapshot.VehicleState.SpeedMps * 3.6).ToString("0.0");
    public string HeadingDeg => (Snapshot.VehicleState.HeadingRad * 180 / Math.PI).ToString("0");
    public string AutoSteerStatus => Snapshot.AutoSteerEnabled ? "AUTO" : "MANUAL";
    public UiState UiState
    {
        get => _uiState;
        private set
        {
            _uiState = value;
            OnPropertyChanged();
        }
    }

    public bool IsUnlocked
    {
        get => _isUnlocked;
        set
        {
            if (_isUnlocked == value) return;
            _isUnlocked = value;
            OnPropertyChanged();
        }
    }

    public bool IsBoundaryEditMode
    {
        get => _isBoundaryEditMode;
        set
        {
            if (_isBoundaryEditMode == value) return;
            _isBoundaryEditMode = value;
            OnPropertyChanged();
        }
    }

    public void SetManualInputs(double speedMps, double steerRad)
    {
        _targetSpeedMps = speedMps;
        _steerInputRad = steerRad;
    }

    public void ToggleAutoSteer() => _bus.Publish(new ToggleAutoSteer());
    public void NextSwath() => _bus.Publish(new NextSwath());
    public void PreviousSwath() => _bus.Publish(new PreviousSwath());

    public void UpdateBoundary(IReadOnlyList<AgNext.Core.Geometry.Vec2> points)
    {
        _bus.Publish(new BoundaryUpdated(points));
    }

    public void SaveLayout()
    {
        _layoutStore.Save(OverlayModules);
    }

    private void Tick()
    {
        _bus.Publish(new ManualControlInput(_targetSpeedMps, _steerInputRad));
        _runtime.Tick();
    }

    private void RegisterBuiltinModules(IUiModuleRegistry registry)
    {
        registry.Register(new MetricModuleViewModel("speed", "Speed", "0.0 km/h", 16, 16, 140, 70, "#51E087"));
        registry.Register(new MetricModuleViewModel("heading", "Heading", "0 deg", 16, 96, 140, 70, "#5CC0FF"));
        registry.Register(new MetricModuleViewModel("auto", "Autosteer", "MANUAL", 16, 176, 140, 70, "#F6C177"));
        registry.Register(new MetricModuleViewModel("swath", "Swath", "0", 16, 256, 140, 70, "#C792EA"));
    }

    private void RegisterPluginModules(IUiModuleRegistry registry)
    {
        foreach (var provider in DiscoverModuleProviders())
        {
            try
            {
                provider.RegisterModules(registry);
            }
            catch
            {
                // Ignore plugin registration errors for now.
            }
        }
    }

    private void RegisterModule(UiModuleViewModel module)
    {
        switch (module.Region)
        {
            case UiModuleRegion.Overlay:
                OverlayModules.Add(module);
                break;
            case UiModuleRegion.RightRail:
                RightRailModules.Add(module);
                break;
            case UiModuleRegion.StatusBar:
                StatusModules.Add(module);
                break;
        }
    }

    private void ApplyLayout(IReadOnlyDictionary<string, UiModuleLayout> layouts)
    {
        if (layouts.Count == 0)
        {
            return;
        }

        foreach (var module in OverlayModules)
        {
            if (!layouts.TryGetValue(module.Id, out var layout))
            {
                continue;
            }

            module.X = layout.X;
            module.Y = layout.Y;
            if (layout.Width > 0)
            {
                module.Width = layout.Width;
            }
            if (layout.Height > 0)
            {
                module.Height = layout.Height;
            }
        }
    }

    private static IEnumerable<IUiModuleProvider> DiscoverModuleProviders()
    {
        var providers = new List<IUiModuleProvider>();
        var seen = new HashSet<Type>();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.IsDynamic)
            {
                continue;
            }

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(type => type != null).ToArray()!;
            }

            foreach (var type in types)
            {
                if (type == null || !typeof(IUiModuleProvider).IsAssignableFrom(type))
                {
                    continue;
                }

                if (type.IsAbstract || type.IsInterface || !seen.Add(type))
                {
                    continue;
                }

                if (type.GetConstructor(Type.EmptyTypes) == null)
                {
                    continue;
                }

                if (Activator.CreateInstance(type) is IUiModuleProvider provider)
                {
                    providers.Add(provider);
                }
            }
        }

        return providers;
    }

    private static void LoadPluginAssemblies()
    {
        var pluginRoot = Path.Combine(AppContext.BaseDirectory, "plugins", "ui");
        if (!Directory.Exists(pluginRoot))
        {
            return;
        }

        foreach (var pluginPath in Directory.EnumerateFiles(pluginRoot, "*.dll"))
        {
            try
            {
                AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(pluginPath));
            }
            catch
            {
                // Skip load failures for now.
            }
        }
    }

    private void UpdateMetricModules()
    {
        foreach (var block in OverlayModules.OfType<MetricModuleViewModel>())
        {
            switch (block.Id)
            {
                case "speed":
                    block.Value = $"{_uiState.Monitoring.SpeedMps * 3.6:0.0} km/h";
                    break;
                case "heading":
                    block.Value = $"{_uiState.Monitoring.HeadingDeg:0} deg";
                    break;
                case "auto":
                    block.Value = AutoSteerStatus;
                    break;
                case "swath":
                    block.Value = Snapshot.ActiveSwathIndex.ToString();
                    break;
            }
        }
    }

    private void UpdateRows()
    {
        var sections = _uiState.SectionState.SectionsOn;
        var rates = _uiState.RateCommand.TargetRates;
        var populations = _uiState.Planter.RowPopulation;

        var rows = _sectionsModule.Rows;
        for (var i = 0; i < rows.Count; i++)
        {
            rows[i].IsOn = i < sections.Length && sections[i];
            rows[i].Rate = i < rates.Length ? rates[i] : 0;
            rows[i].Population = i < populations.Length ? populations[i] : 0;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
