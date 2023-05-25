using System;

namespace AI
{
    public class ScaredAttitude : Attitude
    {
        public ScaredAttitude(Func<float> strengthCalculation) : base(strengthCalculation)
        {
        }
    }
}