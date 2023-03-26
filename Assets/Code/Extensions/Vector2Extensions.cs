using UnityEngine;

namespace Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Floor(this Vector2 v)
        {
            return new(Mathf.Floor(v.x), Mathf.Floor(v.y));
        }
    }
}