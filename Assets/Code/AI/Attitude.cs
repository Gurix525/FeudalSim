using System;
using UnityEngine;

namespace AI
{
    public class Attitude
    {
        private Func<Component, float> _powerCalculation;

        public Component Component { get; }
        public AttitudeType AttitudeType { get; }
        public float Power { get; private set; }

        public Attitude(Component component, AttitudeType attitudeType, Func<Component, float> powerCalculationMethod)
        {
            Component = component;
            AttitudeType = attitudeType;
            _powerCalculation = powerCalculationMethod;
            RecalculatePower();
        }

        public void RecalculatePower()
        {
            if (Component != null)
                Power = _powerCalculation(Component);
            else
                Power = float.NegativeInfinity;
        }
    }
}