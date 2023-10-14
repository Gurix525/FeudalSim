using System;
using UnityEngine;

namespace Controls
{
    public class RaycastHitChangedEventArgs : EventArgs
    {
        public RaycastHit? PreviousRaycastHit { get; }
        public RaycastHit? NewRaycastHit { get; }

        public RaycastHitChangedEventArgs(RaycastHit? previousRaycastHit, RaycastHit? newRaycastHit)
        {
            PreviousRaycastHit = previousRaycastHit;
            NewRaycastHit = newRaycastHit;
        }
    }
}