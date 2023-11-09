using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class Recipe
    {
        private bool _isDiscovered = false;
        [field: SerializeField] public RecipeItem[] Items { get; set; }

        public bool IsEmpty => Items == null ? true : Items.Length == 0;

        public bool IsDiscovered
        {
            get
            {
                //Debug.Log(_isDiscovered);
                //if (_isDiscovered)
                //    return true;
                foreach (var item in Items)
                {
                    if (!Item.GetModel(item.Name).IsDiscovered)
                        return false;
                }
                //_isDiscovered = true;
                return true;
            }
        }

        public override string ToString()
        {
            return string.Join('\n', (object)Items);
        }

        [Serializable]
        public class RecipeItem
        {
            [field: SerializeField] public ItemScriptableObject Item { get; set; }
            [field: SerializeField] public int Count { get; set; }

            public string Name => Item.name;

            public override string ToString()
            {
                return $"{Item.name}: {Count}";
            }
        }
    }
}