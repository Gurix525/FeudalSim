using System;

namespace AI
{
    public class HungryAttitude : Attitude
    {
        public HungryAttitude(Func<float> strengthCalculation) : base(strengthCalculation)
        {
        }
    }
}