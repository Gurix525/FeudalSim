using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class IEnumerableExtensions
    {
        public static Vector2 GetAverage(this IEnumerable<Vector2> vectors)
        {
            float averageX = vectors.Select(vector => vector.x).Sum() / vectors.Count();
            float averageY = vectors.Select(vector => vector.y).Sum() / vectors.Count();
            return new(averageX, averageY);
        }

        public static Vector3 GetAverage(this IEnumerable<Vector3> vectors)
        {
            float averageX = vectors.Select(vector => vector.x).Sum() / vectors.Count();
            float averageY = vectors.Select(vector => vector.y).Sum() / vectors.Count();
            float averageZ = vectors.Select(vector => vector.z).Sum() / vectors.Count();
            return new(averageX, averageY, averageZ);
        }
    }
}