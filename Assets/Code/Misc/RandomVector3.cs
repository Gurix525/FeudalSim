using Extensions;
using UnityEngine;

namespace Misc
{
    public struct RandomVector3
    {
        private readonly Vector3 _vector;

        public static Vector3 One => new RandomVector3(1F, 1F, 1F, true);
        public static Vector3 OneUnsigned => new RandomVector3(1F, 1F, 1F, false);

        public RandomVector3(float max, bool hasNegative = true)
        {
            System.Random random = new();
            float result = (float)random.NextDouble();
            result = result.Remap(0F, 1F, hasNegative ? -max : 0F, max);
            _vector = new(result, result, result);
        }

        public RandomVector3(float min, float max)
        {
            System.Random random = new();
            float result = (float)random.NextDouble();
            result = result.Remap(0F, 1F, min, max);
            _vector = new(result, result, result);
        }

        public RandomVector3(float xMax, float yMax, float zMax, bool hasNegative = true)
        {
            System.Random random = new();
            float resultX = (float)random.NextDouble();
            float resultY = (float)random.NextDouble();
            float resultZ = (float)random.NextDouble();
            resultX = resultX.Remap(0F, 1F, hasNegative ? -xMax : 0F, xMax);
            resultY = resultY.Remap(0F, 1F, hasNegative ? -yMax : 0F, yMax);
            resultZ = resultZ.Remap(0F, 1F, hasNegative ? -zMax : 0F, zMax);
            _vector = new(resultX, resultY, resultZ);
        }

        public RandomVector3(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            System.Random random = new();
            float resultX = (float)random.NextDouble();
            float resultY = (float)random.NextDouble();
            float resultZ = (float)random.NextDouble();
            resultX = resultX.Remap(0F, 1F, xMin, xMax);
            resultY = resultY.Remap(0F, 1F, yMin, yMax);
            resultZ = resultZ.Remap(0F, 1F, zMin, zMax);
            _vector = new(resultX, resultY, resultZ);
        }

        public static implicit operator Vector3(RandomVector3 randomVector)
        {
            return randomVector._vector;
        }
    }
}