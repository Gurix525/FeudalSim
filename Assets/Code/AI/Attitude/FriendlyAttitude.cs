using System;

namespace AI
{
    public class FriendlyAttitude : Attitude
    {
        public FriendlyAttitude(Func<float> strengthCalculation) : base(strengthCalculation)
        {
        }
    }
}