﻿namespace Items
{
    public class ItemReference
    {
        public Container Container { get; }
        public int Index { get; }
        public Item Item => Container[Index];

        public ItemReference(Container container, int index)
        {
            if (container[index] == null)
                return;
            Container = container;
            Index = index;
        }
    }
}