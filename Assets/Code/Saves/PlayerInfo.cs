using System;
using Items;
using Misc;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class PlayerInfo
    {
        public Vector3 Position;
        public ContainerInfo InventoryContainer;
        public ContainerInfo ArmorContainer;

        public PlayerInfo()
        {
        }

        public PlayerInfo(bool hasToInitialize)
        {
            if (hasToInitialize)
                Initialize();
        }

        public void Initialize()
        {
            Position = References.GetReference("Player").transform.position;
            InventoryContainer = new(InventoryCanvas.InventoryContainer);
            ArmorContainer = new(InventoryCanvas.ArmorContainer);
        }
    }
}