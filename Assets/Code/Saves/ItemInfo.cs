using System;
using System.Linq;
using Items;

namespace Saves
{
    [Serializable]
    public class ItemInfo
    {
        public string Model;
        public string[] StatKeys;
        public string[] StatValues;
        public int Count;

        public ItemInfo(Item item)
        {
            Model = item.Model.Name;
            Count = item.Count;
            if (item.HasSpecificStats)
            {
                StatKeys = item.Stats.Keys.ToArray();
                StatValues = item.Stats.Values.ToArray();
            }
            else
            {
                StatKeys = new string[0];
                StatValues = new string[0];
            }
        }

        public static explicit operator Item(ItemInfo itemInfo)
        {
            Item item = Item.Create(itemInfo.Model, itemInfo.Count);
            if (itemInfo.StatKeys.Length > 0)
                item.Stats = itemInfo.StatKeys
                    .Zip(itemInfo.StatValues, (key, value) => new { key, value })
                    .ToDictionary(item => item.key, item => item.value);
            return item;
        }
    }
}