using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class Recipe : ITooltipSource
    {
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

        public Tooltip GetTooltip()
        {
            List<TooltipElement> elements = new();
            foreach (var item in Items)
            {
                TooltipElement element = new($"{item.Count} {item.Name}", TooltipElement.FontType.Normal, item.Sprite);
                elements.Add(element);
            }
            return new(elements.ToArray());
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

            public Sprite Sprite => Item.Sprite;

            public override string ToString()
            {
                return $"{Item.name}: {Count}";
            }
        }
    }
}