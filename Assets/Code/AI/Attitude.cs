using System;
using UnityEngine;

namespace AI
{
    public class Attitude
    {
        private Func<float> _powerCalculation;

        public Component Component { get; }
        public AttitudeType AttitudeType { get; }
        public float Power { get; private set; }

        public Attitude(Component component, AttitudeType attitudeType, Func<float> powerCalculationMethod)
        {
            Component = component;
            AttitudeType = attitudeType;
            _powerCalculation = powerCalculationMethod;
            RecalculatePower();
        }

        public void RecalculatePower()
        {
            Power = _powerCalculation();
        }
    }
}