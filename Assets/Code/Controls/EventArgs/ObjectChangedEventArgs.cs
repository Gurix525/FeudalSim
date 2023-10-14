using System;
using UnityEngine;

namespace Controls
{
    public class ObjectChangedEventArgs : EventArgs
    {
        public GameObject PreviousObject { get; }
        public GameObject NewObject { get; }

        public ObjectChangedEventArgs(GameObject previousObject, GameObject newObject)
        {
            PreviousObject = previousObject;
            NewObject = newObject;
        }
    }
}