using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Average(this Vector3 a, Vector3 b)
        {
            return new((a.x + b.x) / 2F, (a.y + b.y) / 2F, (a.z + b.z) / 2F);
        }

        public static Vector3 Floor(this Vector3 v)
        {
            return new(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
        }

        public static Vector3 Round(this Vector3 v, float accuracy = 1F)
        {
            return new(
                Mathf.Round(v.x / accuracy) * accuracy,
                Mathf.Round(v.y / accuracy) * accuracy,
                Mathf.Round(v.z / accuracy) * accuracy);
        }

        public static Vector3 Clamp(this Vector3 v, float min, float max)
        {
            return new(v.x.Clamp(min, max), v.y.Clamp(min, max), v.z.Clamp(min, max));
        }

        public static Vector3Int ToVector3Int(this Vector3 v)
        {
            return new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

        public static Vector3 RotateAroundPivot(
            this Vector3 point,
            Vector3 pivot,
            Vector3 angles)
        {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }

        public static Vector3 RotateAroundPivot(
            this Vector3 point,
            Vector2 pivot,
            Vector3 angles)
        {
            return Quaternion
                .Euler(angles) * (point - (Vector3)pivot) + (Vector3)pivot;
        }

        public static Vector3 RotateAroundPivot(
            this Vector3 point,
            Vector3 pivot,
            float zAngles)
        {
            return Quaternion
                .Euler(new(0, 0, zAngles)) * (point - pivot) + pivot;
        }

        public static Vector3 RotateAroundPivot(
            this Vector3 point,
            Vector2 pivot,
            float zAngles)
        {
            return Quaternion
                .Euler(new(0, 0, zAngles)) * (point - (Vector3)pivot)
                + (Vector3)pivot;
        }
    }
}