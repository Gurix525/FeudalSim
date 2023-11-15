using System;

namespace Extensions
{
    public static class IntExtensions
    {
        public static bool IsInRangeInclusive(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}