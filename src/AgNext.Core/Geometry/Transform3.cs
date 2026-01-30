namespace AgNext.Core.Geometry;

public readonly struct Transform3
{
    public Vec3 Translation { get; }
    public Quaternion Rotation { get; }

    public Transform3(Vec3 translation, Quaternion rotation)
    {
        Translation = translation;
        Rotation = rotation;
    }

    public static Transform3 Identity => new(new Vec3(0, 0, 0), Quaternion.Identity);

    public Transform3 Combine(Transform3 child)
    {
        var rotated = Rotation.Rotate(child.Translation);
        var translation = Translation + rotated;
        var rotation = Rotation * child.Rotation;
        return new Transform3(translation, rotation);
    }

    public Vec3 TransformPoint(Vec3 point)
    {
        return Translation + Rotation.Rotate(point);
    }

    public Transform3 Inverse()
    {
        var invRot = Rotation.Conjugate().Normalize();
        var invTrans = invRot.Rotate(-Translation);
        return new Transform3(invTrans, invRot);
    }
}
