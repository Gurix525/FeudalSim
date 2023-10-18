using System;
using UnityEngine;

namespace Controls
{
    public class PassedRotationEventArgs : EventArgs
    {
        public float Yaw { get; }
        public float Pitch { get; }
        public Vector3 PitchAxis { get; }

        public PassedRotationEventArgs(float yaw, float pitch, Vector3 pitchAxis)
        {
            Yaw = yaw;
            Pitch = pitch;
            PitchAxis = pitchAxis;
        }
    }
}