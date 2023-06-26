using System;
using UnityEngine;

namespace Maths
{
    public class Curve
    {
        private float? _approximateLength;

        public Vector3 StartPosition { get; }
        public Vector3 TargetPosition { get; }
        public Vector3 ControlPoint { get; }

        public float ApproximateLength => _approximateLength ??= GetLength();

        public Curve(Vector3 start, Vector3 target, Vector3 control)
        {
            StartPosition = start;
            TargetPosition = target;
            ControlPoint = control;
        }

        public Vector3 GetPoint(float t)
        {
            return Mathf.Pow(1 - t, 2) * StartPosition
                + 2 * (1 - t) * t * ControlPoint
                + t * t * TargetPosition;
        }

        public override bool Equals(object obj)
        {
            return obj is Curve curve &&
                   StartPosition.Equals(curve.StartPosition) &&
                   TargetPosition.Equals(curve.TargetPosition) &&
                   ControlPoint.Equals(curve.ControlPoint);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartPosition, TargetPosition, ControlPoint);
        }

        private float GetLength()
        {
            float length = 0F;
            for (int i = 0; i < 11; i++)
            {
                length += (GetPoint((i + 1F) / 11F) - GetPoint(i / 11F)).magnitude;
            }
            return length;
        }
    }
}