using UnityEngine;

namespace Math
{
    public class Curve
    {
        public Vector3 StartPosition { get; }
        public Vector3 TargetPosition { get; }
        public Vector3 ControlPoint { get; }

        public Curve(Vector3 start, Vector3 target, Vector3 control)
        {
            StartPosition = start;
            TargetPosition = target;
            ControlPoint = control;
        }
    }
}