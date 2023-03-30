using System;
using System.Collections.Generic;

namespace Items
{
    public class Item
    {
        private ItemModel _model;
        private Dictionary<string, float> _stats;

        public int Count { get; set; }
        public string Name => _model.Name;
        public string Description => _model.Description;
        public int MaxStack => _model.MaxStack;
        public Dictionary<string, float> Stats => _stats ?? _model.Stats;

        public Item(ItemModel model, int count, Dictionary<string, float> stats = null)
        {
            if (count > model.MaxStack)
                throw new ArgumentOutOfRangeException(
                    "Count nie może być większe niż MaxStack");
            _model = model;
            Count = count;
            _stats = stats;
        }

        public override string ToString()
        {
            return $"{Name}: {Count}";
        }
    }
}