using System;

namespace AI
{
    public class NeutralAttitude : Attitude
    {
        public NeutralAttitude(Func<float> strengthCalculation) : base(strengthCalculation)
        {
        }
    }
}