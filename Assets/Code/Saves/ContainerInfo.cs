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

        public static explicit operator Container(ContainerInfo containerInfo)
        {
            Container container = new(containerInfo.Size, containerInfo.Lock);
            container.SetItems(containerInfo.Items.Select(item => (Item)item).ToArray());
            return container;
        }
    }
}