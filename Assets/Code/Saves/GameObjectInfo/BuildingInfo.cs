using System;
using Buildings;
using Controls;

namespace Saves
{
    [Serializable]
    public class BuildingInfo : GameObjectInfo
    {
        public string Name;

        public BuildingInfo()
        {
        }

        public BuildingInfo(Building building) : base(building)
        {
            Initialize(building);
        }

        public void Initialize(Building building)
        {
            // To be added
            base.Initialize(building);
            Name = building.Name;
        }
    }
}