using System.Collections.Generic;

namespace Items
{
    public class ItemModel
    {
        public string Name { get; }
        public string Description { get; }
        public int MaxStack { get; }
        public Dictionary<string, float> Stats { get; }

        public ItemModel(string name, int maxStack, string description = "", Dictionary<string, float> stats = null)
        {
            Name = name;
            Description = description;
            MaxStack = maxStack;
            Stats = stats;
        }
    }
}