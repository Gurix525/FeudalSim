using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 GetAverage(this Vector3 a, Vector3 b)
    {
        return new((a.x + b.x) / 2F, (a.y + b.y) / 2F, (a.z + b.z) / 2F);
    }
}