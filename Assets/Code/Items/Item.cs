using System;
using System.Collections.Generic;

namespace Items
{
    public class Item
    {
        #region Fields

        private ItemModel _model;
        private Dictionary<string, float> _stats;

        private static Dictionary<string, ItemModel> _itemModels = new()
        {
            { "Stone", new("Stone") },
            { "Wood", new("Wood") },
            { "Sword", new("Sword", 1) },
            { "Axe", new("Axe", 1) }
        };

        #endregion Fields

        #region Properties

        public int Count { get; set; }
        public string Name => _model.Name;
        public string Description => _model.Description;
        public int MaxStack => _model.MaxStack;
        public Dictionary<string, float> Stats => _stats ?? _model.Stats;

        #endregion Properties

        #region Constructors

        private Item(ItemModel model, int count = 1, Dictionary<string, float> stats = null)
        {
            if (count > model.MaxStack)
                throw new ArgumentOutOfRangeException(
                    "Count przemiotu nie może być większe niż MaxStack");
            _model = model;
            Count = count;
            _stats = stats;
        }

        #endregion Constructors

        #region Public

        public Item Clone(int count = 0)
        {
            return new(_model, count == 0 ? Count : count, _stats);
        }

        public override string ToString()
        {
            return $"{Name}: {Count}";
        }

        public static Item Create(
            string name,
            int count = 1,
            Dictionary<string, float> stats = null)
        {
            return new(_itemModels[name], count, stats);
        }

        #endregion Public
    }
}