using System.Collections.Generic;

namespace Items
{
    public class ItemModel
    {
        #region Properties

        public string Name { get; }
        public string Description { get; }
        public int MaxStack { get; }
        public Dictionary<string, float> Stats { get; }

        #endregion Properties

        #region Constructors

        public ItemModel(string name, int maxStack = 10, string description = "", Dictionary<string, float> stats = null)
        {
            Name = name;
            Description = description;
            MaxStack = maxStack;
            Stats = stats;
        }

        #endregion Constructors
    }
}