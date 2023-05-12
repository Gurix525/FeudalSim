using System;
using System.ComponentModel;
using Buildings;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class GameObjectInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public GameObjectInfo()
        {
        }

        public GameObjectInfo(UnityEngine.Component component)
        {
            Initialize(component);
        }

        public void Initialize(UnityEngine.Component component)
        {
            Position = component.transform.position;
            Rotation = component.transform.rotation;
        }
    }
}