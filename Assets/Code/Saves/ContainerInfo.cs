using System;
using System.Linq;
using Items;

namespace Saves
{
    [Serializable]
    public class ContainerInfo
    {
        public int Size;
        public string Lock;
        public ItemInfo[] Items;

        public ContainerInfo()
        {
        }

        public ContainerInfo(Container container)
        {
            Initialize(container);
        }

        public void Initialize(Container container)
        {
            Size = container.Size;
            Lock = container.Lock;
            Items = container.Items
                .Select(item => item != null ? new ItemInfo(item) : null)
                .ToArray();
        }
    }
}