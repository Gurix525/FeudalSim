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
        public ItemInfo CursorItem;

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
            InventoryContainer = new(Equipment.InventoryContainer);
            ArmorContainer = new(Equipment.ArmorContainer);
            CursorItem = new(Controls.Cursor.Container[0]);
        }
    }
}