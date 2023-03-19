using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 GetAverage(this Vector3 a, Vector3 b)
    {
        return new((a.x + b.x) / 2F, (a.y + b.y) / 2F, (a.z + b.z) / 2F);
    }

    public static Vector3 Floor(this Vector3 v)
    {
        return new(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
    }
}