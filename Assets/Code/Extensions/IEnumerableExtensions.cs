using System;
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

        public static T RandomElementByWeight<T>(
            this IEnumerable<T> sequence, Func<T, float> weightSelector)
        {
            float totalWeight = sequence.Sum(weightSelector);
            float itemWeightIndex = (float)new System.Random().NextDouble() * totalWeight;
            float currentWeightIndex = 0;

            foreach (var item in from weightedItem in sequence
                                 select new
                                 {
                                     Value = weightedItem,
                                     Weight = weightSelector(weightedItem)
                                 })
            {
                currentWeightIndex += item.Weight;

                if (currentWeightIndex >= itemWeightIndex)
                    return item.Value;
            }

            return default(T);
        }
    }
}