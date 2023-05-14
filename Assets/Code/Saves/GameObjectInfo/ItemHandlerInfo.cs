﻿using System;
using Items;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class ItemHandlerInfo : GameObjectInfo
    {
        public ContainerInfo Container;

        public Item Item => (Item)Container.Items[0];

        public ItemHandlerInfo()
        {
        }

        public ItemHandlerInfo(ItemHandler itemHandler) : base(itemHandler)
        {
            Initialize(itemHandler);
        }

        public void Initialize(ItemHandler itemHandler)
        {
            Container = new(itemHandler.Container);
        }
    }
}