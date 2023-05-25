using System;

namespace AI
{
    public abstract class Attitude
    {
        private Func<float> _strengthCalculation;

        public float Strength { get; set; }

        public Attitude(Func<float> strengthCalculationMethod)
        {
            _strengthCalculation = strengthCalculationMethod;
            RecalculateStrength();
        }

        public void RecalculateStrength()
        {
            Strength = _strengthCalculation();
        }

        public override string ToString()
        {
            return $"{GetType()}: {Strength}";
        }
    }
}