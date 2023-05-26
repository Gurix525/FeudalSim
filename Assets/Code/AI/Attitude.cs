using System;

namespace AI
{
    public class Attitude
    {
        private Func<float> _strengthCalculation;

        public AttitudeType AttitudeType { get; }
        public float Strength { get; set; }

        public Attitude(AttitudeType attitudeType, Func<float> strengthCalculationMethod)
        {
            AttitudeType = attitudeType;
            _strengthCalculation = strengthCalculationMethod;
            RecalculateStrength();
        }

        public void RecalculateStrength()
        {
            Strength = _strengthCalculation();
        }
    }
}