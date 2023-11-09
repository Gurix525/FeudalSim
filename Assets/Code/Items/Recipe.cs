using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class Recipe
    {
        [field: SerializeField] public RecipeItem[] Items { get; set; }

        private bool _isDiscovered;

        public bool IsEmpty => Items == null ? true : Items.Length == 0;

        public bool IsDiscovered
        {
            get
            {
                if (_isDiscovered)
                    return true;
                foreach (var item in Items)
                {
                    if (!Item.GetModel(item.Item.name).IsDiscovered)
                        return false;
                }
                _isDiscovered = true;
                return true;
            }
        }

        [Serializable]
        public class RecipeItem
        {
            [field: SerializeField] public ItemScriptableObject Item { get; set; }
            [field: SerializeField] public int Count { get; set; }
        }
    }
}