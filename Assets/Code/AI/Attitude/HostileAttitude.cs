using System;

namespace AI
{
    public class HostileAttitude : Attitude
    {
        public HostileAttitude(Func<float> strengthCalculation) : base(strengthCalculation)
        {
        }
    }
}