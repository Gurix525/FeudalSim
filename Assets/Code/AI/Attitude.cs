using System;
using UnityEngine;

namespace AI
{
    public class Attitude
    {
        private Func<float> _strengthCalculation;

        public Component Component { get; }
        public AttitudeType AttitudeType { get; }
        public float Strength { get; private set; }

        public Attitude(Component component, AttitudeType attitudeType, Func<float> strengthCalculationMethod)
        {
            Component = component;
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