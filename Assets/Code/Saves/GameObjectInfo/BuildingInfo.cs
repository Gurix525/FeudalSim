using System;
using Buildings;
using Controls;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class BuildingInfo : GameObjectInfo
    {
        public ItemInfo BackingItem;
        public BuildingMode BuildingMode;

        public BuildingInfo(Building building) : base(building)
        {
            BackingItem = new(building.BackingItem);
            BuildingMode = building.BuildingMode;
        }
    }
}