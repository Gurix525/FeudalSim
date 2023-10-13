namespace UI
{
    public class FloatChangedEventArgs
    {
        public float PreviousValue { get; }
        public float NewValue { get; }

        public FloatChangedEventArgs(float previousValue, float newValue)
        {
            PreviousValue = previousValue;
            NewValue = newValue;
        }
    }
}