using System;
using Items;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class ItemHandlerInfo : GameObjectInfo
    {
        public ContainerInfo Container;

        public ItemHandlerInfo(ItemHandler handler) : base(handler)
        {
            Container = new(handler.Container);
        }
    }
}