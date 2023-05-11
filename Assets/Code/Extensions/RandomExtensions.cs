using System;

namespace MyNamespace
{
    public static class RandomExtensions
    {
        public static long NextLong(this Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException();
            ulong uRange = (ulong)(max - min);
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            }
            while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);
            return (long)(ulongRand % uRange) + min;
        }

        public static long NextLong(this Random random, long max)
        {
            return random.NextLong(0, max);
        }

        public static long NextLong(this Random random)
        {
            return random.NextLong(long.MinValue, long.MaxValue);
        }
    }
}