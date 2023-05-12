using System;
using Buildings;
using Controls;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class BuildingInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public ItemInfo BackingItem;
        public BuildingMode BuildingMode;

        public BuildingInfo(Building building)
        {
            Position = building.transform.position;
            Rotation = building.transform.rotation;
            BackingItem = new(building.BackingItem);
            BuildingMode = building.BuildingMode;
        }
    }
}