using System;
using Buildings;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class GameObjectInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public GameObjectInfo(Component component)
        {
            Position = component.transform.position;
            Rotation = component.transform.rotation;
        }
    }
}