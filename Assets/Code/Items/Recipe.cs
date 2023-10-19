using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class Recipe
    {
        [field: SerializeField] public RecipeItem[] Items { get; set; }

        [Serializable]
        public class RecipeItem
        {
            [field: SerializeField] public ItemScriptableObject Item { get; set; }
            [field: SerializeField] public int Count { get; set; }
        }
    }
}