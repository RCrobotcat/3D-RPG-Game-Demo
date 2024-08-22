using UnityEngine;

public static class ExtensionMethods
{
    private const float dotThreshole = 0.5f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        // dot product: a.b = |a| * |b| * cos(theta)
        float dot = Vector3.Dot(transform.forward, vectorToTarget);
        return dot >= dotThreshole;
    }
}
