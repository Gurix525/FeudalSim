using System;
using Items;

namespace Controls
{
    public class ItemReferenceChangedEventArgs : EventArgs
    {
        public ItemReference PreviousReference { get; }
        public ItemReference NewReference { get; }

        public ItemReferenceChangedEventArgs(ItemReference previousReference, ItemReference newReference)
        {
            PreviousReference = previousReference;
            NewReference = newReference;
        }
    }
}