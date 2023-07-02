using System;

namespace Extensions
{
    public static class FloatExtensions
    {
        public static float Remap(this float value, float startA, float startB, float targetA, float targetB)
        {
            return (value - startA) / (startB - startA) * (targetB - targetA) + targetA;
        }

        public static float Clamp(this float value, float min, float max)
        {
            return Math.Clamp(value, min, max);
        }

        public static float Round(this float value)
        {
            return MathF.Round(value);
        }

        public static float Floor(this float value)
        {
            return MathF.Floor(value);
        }

        public static float Ceiling(this float value)
        {
            return MathF.Ceiling(value);
        }
    }
}