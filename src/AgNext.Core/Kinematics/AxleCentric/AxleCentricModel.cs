namespace AgNext.Core.Kinematics.AxleCentric;

using AgNext.Core.Geometry;

public sealed class FrameNode
{
    public string Id { get; }
    public string Type { get; }
    public string? ParentId { get; }
    public Transform3 Local { get; set; }
    public Transform3 BaseLocal { get; }
    public Transform3 World { get; set; }
    public AxleConfig? Axle { get; }
    public DrawbarConfig? Drawbar { get; }

    public FrameNode(string id, string type, string? parentId, Transform3 local, AxleConfig? axle, DrawbarConfig? drawbar)
    {
        Id = id;
        Type = type;
        ParentId = parentId;
        Local = local;
        BaseLocal = local;
        Axle = axle;
        Drawbar = drawbar;
        World = Transform3.Identity;
    }
}

public sealed class AttachmentNode
{
    public string Id { get; }
    public string ParentId { get; }
    public string Type { get; }
    public Transform3 Local { get; }
    public Transform3 World { get; set; }

    public AttachmentNode(string id, string parentId, string type, Transform3 local)
    {
        Id = id;
        ParentId = parentId;
        Type = type;
        Local = local;
        World = Transform3.Identity;
    }
}

public sealed class AxleCentricModel
{
    private readonly Dictionary<string, FrameNode> _nodes;
    private readonly Dictionary<string, AttachmentNode> _attachments;
    private readonly RowConfig _rowConfig;
    private string _rootId;

    public AxleCentricModel(Dictionary<string, FrameNode> nodes, Dictionary<string, AttachmentNode> attachments, RowConfig rowConfig)
    {
        _nodes = nodes;
        _attachments = attachments;
        _rowConfig = rowConfig;
        _rootId = nodes.Keys.FirstOrDefault() ?? string.Empty;
    }

    public static AxleCentricModel FromConfig(AxleCentricConfig config)
    {
        var nodes = new Dictionary<string, FrameNode>(StringComparer.OrdinalIgnoreCase);
        foreach (var node in config.Nodes)
        {
            var translation = new Vec3(node.Translation[0], node.Translation[1], node.Translation[2]);
            var rotation = new Quaternion(node.Rotation[0], node.Rotation[1], node.Rotation[2], node.Rotation[3]);
            nodes[node.Id] = new FrameNode(node.Id, node.Type, node.ParentId, new Transform3(translation, rotation), node.Axle, node.Drawbar);
        }

        var attachments = new Dictionary<string, AttachmentNode>(StringComparer.OrdinalIgnoreCase);
        foreach (var attachment in config.Attachments)
        {
            var translation = new Vec3(attachment.Translation[0], attachment.Translation[1], attachment.Translation[2]);
            var rotation = new Quaternion(attachment.Rotation[0], attachment.Rotation[1], attachment.Rotation[2], attachment.Rotation[3]);
            attachments[attachment.Id] = new AttachmentNode(attachment.Id, attachment.ParentId, attachment.Type, new Transform3(translation, rotation));
        }

        var model = new AxleCentricModel(nodes, attachments, config.Rows)
        {
            _rootId = config.RootId
        };
        return model;
    }

    public void SetRootPose(string rootId, Vec3 position, double yawRad)
    {
        SetRootPose(rootId, position, Quaternion.FromYaw(yawRad));
    }

    public void SetRootPose(string rootId, Vec3 position, Quaternion rotation)
    {
        if (_nodes.TryGetValue(rootId, out var root))
        {
            root.World = new Transform3(position, rotation);
            _rootId = rootId;
        }
        UpdateWorldTransforms();
    }

    public bool TryGetNode(string id, out FrameNode node) => _nodes.TryGetValue(id, out node!);

    public IEnumerable<FrameNode> Nodes => _nodes.Values;

    public IEnumerable<FrameNode> GetChildren(string parentId)
    {
        return _nodes.Values.Where(n => string.Equals(n.ParentId, parentId, StringComparison.OrdinalIgnoreCase));
    }

    public void SetLocalYaw(string id, double yawRad)
    {
        if (_nodes.TryGetValue(id, out var node))
        {
            node.Local = new Transform3(node.BaseLocal.Translation, node.BaseLocal.Rotation * Quaternion.FromYaw(yawRad));
        }
    }

    public void Recalculate() => UpdateWorldTransforms();

    public Transform3 GetWorldTransform(string id)
    {
        return _nodes.TryGetValue(id, out var node) ? node.World : Transform3.Identity;
    }

    public Transform3? GetAttachmentTransform(string id)
    {
        return _attachments.TryGetValue(id, out var attachment) ? attachment.World : null;
    }

    public Pose2D GetPose2D(string id)
    {
        var world = GetWorldTransform(id);
        return new Pose2D(new Vec2(world.Translation.X, world.Translation.Y), world.Rotation.ToYaw());
    }

    public IReadOnlyList<Vec2> GetRowCenters()
    {
        if (!_nodes.ContainsKey(_rowConfig.ParentId))
        {
            return Array.Empty<Vec2>();
        }

        var frame = GetWorldTransform(_rowConfig.ParentId);
        var yaw = frame.Rotation.ToYaw();
        var forward = Vec2.FromAngle(yaw);
        var left = Vec2.FromAngle(yaw + Math.PI / 2);
        var origin = new Vec2(frame.Translation.X, frame.Translation.Y);

        var centers = new Vec2[_rowConfig.RowCount];
        var half = (_rowConfig.RowCount - 1) / 2.0;
        for (var i = 0; i < _rowConfig.RowCount; i++)
        {
            var lateral = (i - half) * _rowConfig.RowSpacingMeters;
            centers[i] = origin + (left * lateral);
        }
        return centers;
    }

    public IReadOnlyList<Vec2> GetRowShutoffPoints()
    {
        if (!_nodes.ContainsKey(_rowConfig.ParentId))
        {
            return Array.Empty<Vec2>();
        }

        var frame = GetWorldTransform(_rowConfig.ParentId);
        var yaw = frame.Rotation.ToYaw();
        var forward = Vec2.FromAngle(yaw);
        var left = Vec2.FromAngle(yaw + Math.PI / 2);
        var origin = new Vec2(frame.Translation.X, frame.Translation.Y);

        var points = new Vec2[_rowConfig.RowCount];
        var half = (_rowConfig.RowCount - 1) / 2.0;
        for (var i = 0; i < _rowConfig.RowCount; i++)
        {
            var lateral = (i - half) * _rowConfig.RowSpacingMeters;
            points[i] = origin + (left * lateral) + forward * _rowConfig.ShutoffOffsetMeters;
        }
        return points;
    }

    private void UpdateWorldTransforms()
    {
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var node in _nodes.Values)
        {
            UpdateNode(node);
        }

        foreach (var attachment in _attachments.Values)
        {
            if (_nodes.TryGetValue(attachment.ParentId, out var parent))
            {
                attachment.World = parent.World.Combine(attachment.Local);
            }
        }

        void UpdateNode(FrameNode node)
        {
            if (!visited.Add(node.Id))
            {
                return;
            }

            if (node.ParentId != null && _nodes.TryGetValue(node.ParentId, out var parent))
            {
                UpdateNode(parent);
                node.World = parent.World.Combine(node.Local);
            }
            else if (node.Id != _rootId)
            {
                node.World = node.Local;
            }
        }
    }
}
