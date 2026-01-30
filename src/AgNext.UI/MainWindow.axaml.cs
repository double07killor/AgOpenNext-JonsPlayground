using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using AgNext.UI.ViewModels;

namespace AgNext.UI;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;
    private readonly HashSet<Key> _keys = new();
    private UiModuleViewModel? _dragModule;
    private Point _dragOffset;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel();
        DataContext = _viewModel;

        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;
        Closing += OnClosing;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        var isNewPress = _keys.Add(e.Key);
        if (isNewPress)
        {
            if (e.Key == Key.Space)
            {
                _viewModel.ToggleAutoSteer();
            }
            else if (e.Key == Key.Q)
            {
                _viewModel.PreviousSwath();
            }
            else if (e.Key == Key.E)
            {
                _viewModel.NextSwath();
            }
        }

        ApplyInputs();
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        _keys.Remove(e.Key);
        ApplyInputs();
    }

    private void ApplyInputs()
    {
        var speed = 0.0;
        if (_keys.Contains(Key.W))
        {
            speed = 6.0;
        }
        else if (_keys.Contains(Key.S))
        {
            speed = -2.0;
        }

        var steer = 0.0;
        if (_keys.Contains(Key.A))
        {
            steer = 0.4;
        }
        else if (_keys.Contains(Key.D))
        {
            steer = -0.4;
        }

        _viewModel.SetManualInputs(speed, steer);
    }

    private void OnModulePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!_viewModel.IsUnlocked)
        {
            return;
        }

        if (sender is not Control control || control.DataContext is not UiModuleViewModel module)
        {
            return;
        }

        if (!module.IsMovable || module.Region != UiModuleRegion.Overlay)
        {
            return;
        }

        _dragModule = module;
        var position = e.GetPosition(control);
        _dragOffset = position;
        e.Pointer.Capture(control);
    }

    private void OnModulePointerMoved(object? sender, PointerEventArgs e)
    {
        if (_dragModule == null || sender is not Control control)
        {
            return;
        }

        var parent = control.GetVisualParent<Control>();
        if (parent == null)
        {
            return;
        }

        var pos = e.GetPosition(parent);
        _dragModule.X = Math.Max(0, pos.X - _dragOffset.X);
        _dragModule.Y = Math.Max(0, pos.Y - _dragOffset.Y);
    }

    private void OnModulePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        e.Pointer.Capture(null);
        _dragModule = null;
        _viewModel.SaveLayout();
    }

    private void OnBoundaryEdited(object? sender, IReadOnlyList<AgNext.Core.Geometry.Vec2> points)
    {
        _viewModel.UpdateBoundary(points);
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        _viewModel.SaveLayout();
    }
}
