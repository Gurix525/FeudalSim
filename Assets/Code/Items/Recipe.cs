using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class Recipe
    {
        [field: SerializeField] public RecipeItem[] Items { get; set; }

        public bool IsEmpty => Items == null ? true : Items.Length == 0;

        [Serializable]
        public class RecipeItem
        {
            [field: SerializeField] public ItemScriptableObject Item { get; set; }
            [field: SerializeField] public int Count { get; set; }
        }
    }
}