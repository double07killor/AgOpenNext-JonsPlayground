namespace AgNext.UI.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using AgNext.Core.Geometry;
using AgNext.Core.Simulation;
using System.Linq;

public sealed class MapView : Control
{
    public static readonly StyledProperty<SimulationSnapshot?> SnapshotProperty =
        AvaloniaProperty.Register<MapView, SimulationSnapshot?>(nameof(Snapshot));

    public static readonly StyledProperty<bool> IsEditModeProperty =
        AvaloniaProperty.Register<MapView, bool>(nameof(IsEditMode));

    public SimulationSnapshot? Snapshot
    {
        get => GetValue(SnapshotProperty);
        set => SetValue(SnapshotProperty, value);
    }

    public bool IsEditMode
    {
        get => GetValue(IsEditModeProperty);
        set => SetValue(IsEditModeProperty, value);
    }

    public event EventHandler<IReadOnlyList<Vec2>>? BoundaryEdited;

    private readonly List<Vec2> _editableBoundary = new();
    private int? _dragIndex;
    private double _scale;
    private double _minX;
    private double _minY;
    private double _padding;

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var snapshot = Snapshot;
        var boundary = GetBoundary(snapshot);
        if (boundary.Count == 0)
        {
            return;
        }

        var bounds = GetBounds(boundary);
        _padding = 20;
        var scaleX = Math.Max(1, (Bounds.Width - _padding * 2) / (bounds.MaxX - bounds.MinX));
        var scaleY = Math.Max(1, (Bounds.Height - _padding * 2) / (bounds.MaxY - bounds.MinY));
        _scale = Math.Min(scaleX, scaleY);
        _minX = bounds.MinX;
        _minY = bounds.MinY;

        Point ToPoint(Vec2 v)
        {
            var x = (v.X - bounds.MinX) * _scale + _padding;
            var y = (v.Y - bounds.MinY) * _scale + _padding;
            return new Point(x, Bounds.Height - y);
        }

        DrawBoundary(context, boundary, ToPoint);
        DrawBoundaryHandles(context, boundary, ToPoint);
        DrawSwaths(context, snapshot?.Swaths ?? Array.Empty<AgNext.Core.Guidance.GuidancePath>(), snapshot?.ActiveSwathIndex ?? 0, ToPoint);
        DrawPreview(context, snapshot?.PreviewPoints ?? Array.Empty<Vec2>(), ToPoint);
        if (snapshot != null)
        {
            DrawVehicle(context, snapshot, ToPoint);
            DrawRows(context, snapshot, ToPoint);
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SnapshotProperty)
        {
            if (!IsEditMode)
            {
                SyncBoundaryFromSnapshot();
            }
            InvalidateVisual();
        }
        else if (change.Property == IsEditModeProperty)
        {
            if (!IsEditMode)
            {
                SyncBoundaryFromSnapshot();
            }
            InvalidateVisual();
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!IsEditMode)
        {
            return;
        }

        var point = e.GetPosition(this);
        var world = ToWorld(point);
        var hitIndex = FindHandle(point);
        if (hitIndex.HasValue)
        {
            _dragIndex = hitIndex;
        }
        else
        {
            _editableBoundary.Add(world);
            _dragIndex = _editableBoundary.Count - 1;
            BoundaryEdited?.Invoke(this, _editableBoundary.ToArray());
        }

        e.Pointer.Capture(this);
        InvalidateVisual();
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (!IsEditMode || _dragIndex == null)
        {
            return;
        }

        var world = ToWorld(e.GetPosition(this));
        _editableBoundary[_dragIndex.Value] = world;
        BoundaryEdited?.Invoke(this, _editableBoundary.ToArray());
        InvalidateVisual();
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (!IsEditMode)
        {
            return;
        }

        _dragIndex = null;
        e.Pointer.Capture(null);
    }

    private void SyncBoundaryFromSnapshot()
    {
        _editableBoundary.Clear();
        var snapshot = Snapshot;
        if (snapshot != null && snapshot.Boundary.Count > 0)
        {
            _editableBoundary.AddRange(snapshot.Boundary);
        }
    }

    private IReadOnlyList<Vec2> GetBoundary(SimulationSnapshot? snapshot)
    {
        if (_editableBoundary.Count > 0)
        {
            return _editableBoundary;
        }
        return snapshot?.Boundary ?? Array.Empty<Vec2>();
    }

    private int? FindHandle(Point point)
    {
        const double radius = 8;
        for (var i = 0; i < _editableBoundary.Count; i++)
        {
            var handle = ToPoint(_editableBoundary[i]);
            var dx = handle.X - point.X;
            var dy = handle.Y - point.Y;
            if (Math.Sqrt(dx * dx + dy * dy) <= radius)
            {
                return i;
            }
        }
        return null;
    }

    private Vec2 ToWorld(Point point)
    {
        var x = (point.X - _padding) / _scale + _minX;
        var y = ((Bounds.Height - point.Y) - _padding) / _scale + _minY;
        return new Vec2(x, y);
    }

    private Point ToPoint(Vec2 v)
    {
        var x = (v.X - _minX) * _scale + _padding;
        var y = (v.Y - _minY) * _scale + _padding;
        return new Point(x, Bounds.Height - y);
    }

    private static void DrawBoundary(DrawingContext context, IReadOnlyList<Vec2> boundary, Func<Vec2, Point> transform)
    {
        if (boundary.Count < 2)
        {
            return;
        }

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(transform(boundary[0]), true);
            for (var i = 1; i < boundary.Count; i++)
            {
                ctx.LineTo(transform(boundary[i]));
            }
            ctx.LineTo(transform(boundary[0]));
        }

        context.DrawGeometry(null, new Pen(Brushes.ForestGreen, 2), geometry);
    }

    private static void DrawBoundaryHandles(DrawingContext context, IReadOnlyList<Vec2> boundary, Func<Vec2, Point> transform)
    {
        foreach (var point in boundary)
        {
            context.DrawEllipse(Brushes.DarkSlateBlue, new Pen(Brushes.LightSteelBlue, 1), transform(point), 4, 4);
        }
    }

    private static void DrawSwaths(DrawingContext context, IReadOnlyList<AgNext.Core.Guidance.GuidancePath> swaths, int activeIndex, Func<Vec2, Point> transform)
    {
        for (var i = 0; i < swaths.Count; i++)
        {
            var points = swaths[i].Points;
            if (points.Count < 2)
            {
                continue;
            }

            var geometry = new StreamGeometry();
            using (var ctx = geometry.Open())
            {
                ctx.BeginFigure(transform(points[0]), false);
                for (var p = 1; p < points.Count; p++)
                {
                    ctx.LineTo(transform(points[p]));
                }
            }

            var pen = i == activeIndex ? new Pen(Brushes.Cyan, 2) : new Pen(Brushes.DimGray, 1);
            context.DrawGeometry(null, pen, geometry);
        }
    }

    private static void DrawPreview(DrawingContext context, IReadOnlyList<Vec2> preview, Func<Vec2, Point> transform)
    {
        if (preview.Count < 2)
        {
            return;
        }

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(transform(preview[0]), false);
            for (var i = 1; i < preview.Count; i++)
            {
                ctx.LineTo(transform(preview[i]));
            }
        }

        var dash = new DashStyle(new double[] { 4, 4 }, 0);
        context.DrawGeometry(null, new Pen(Brushes.LightBlue, 1, dashStyle: dash), geometry);
    }

    private static void DrawVehicle(DrawingContext context, SimulationSnapshot snapshot, Func<Vec2, Point> transform)
    {
        var pose = snapshot.VehicleState.Pose;
        var forward = Vec2.FromAngle(pose.HeadingRad);
        var left = Vec2.FromAngle(pose.HeadingRad + Math.PI / 2);
        var center = pose.Position;
        var nose = center + forward * 3.0;
        var tailLeft = center - forward * 2.0 + left * 1.2;
        var tailRight = center - forward * 2.0 - left * 1.2;

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(transform(nose), true);
            ctx.LineTo(transform(tailLeft));
            ctx.LineTo(transform(tailRight));
            ctx.LineTo(transform(nose));
        }

        context.DrawGeometry(Brushes.Gold, new Pen(Brushes.Goldenrod, 1.5), geometry);

        if (snapshot.RowCenters.Count > 0)
        {
            var leftMost = snapshot.RowCenters.MaxBy(p => Dot(p - snapshot.ImplementPose.Position, snapshot.ImplementPose.Left));
            var rightMost = snapshot.RowCenters.MinBy(p => Dot(p - snapshot.ImplementPose.Position, snapshot.ImplementPose.Left));
            context.DrawLine(new Pen(Brushes.OrangeRed, 2), transform(leftMost), transform(rightMost));
        }
    }

    private static void DrawRows(DrawingContext context, SimulationSnapshot snapshot, Func<Vec2, Point> transform)
    {
        for (var i = 0; i < snapshot.RowCenters.Count; i++)
        {
            var center = transform(snapshot.RowCenters[i]);
            var on = i < snapshot.SectionStates.Length && snapshot.SectionStates[i];
            var brush = on ? Brushes.LimeGreen : Brushes.DarkRed;
            context.DrawEllipse(brush, null, center, 3, 3);

            if (i < snapshot.RowShutoffPoints.Count)
            {
                var shutoff = transform(snapshot.RowShutoffPoints[i]);
                context.DrawEllipse(null, new Pen(Brushes.WhiteSmoke, 1), shutoff, 4, 4);
            }
        }
    }

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBounds(IReadOnlyList<Vec2> points)
    {
        var minX = points.Min(p => p.X);
        var maxX = points.Max(p => p.X);
        var minY = points.Min(p => p.Y);
        var maxY = points.Max(p => p.Y);
        return (minX, maxX, minY, maxY);
    }

    private static double Dot(Vec2 a, Vec2 b) => (a.X * b.X) + (a.Y * b.Y);
}
