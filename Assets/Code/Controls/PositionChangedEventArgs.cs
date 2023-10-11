using System;
using UnityEngine;

namespace Controls
{
    public class PositionChangedEventArgs : EventArgs
    {
        public Vector3? PreviousPosition { get; }
        public Vector3? NewPosition { get; }

        public PositionChangedEventArgs(Vector3? previousPosition, Vector3? newPosition)
        {
            PreviousPosition = previousPosition;
            NewPosition = newPosition;
        }
    }
}