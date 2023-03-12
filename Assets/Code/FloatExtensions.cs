public static class FloatExtensions
{
    public static float Remap(this float value, float startA, float startB, float targetA, float targetB)
    {
        return (value - startA) / (startB - startA) * (targetB - targetA) + targetA;
    }
}