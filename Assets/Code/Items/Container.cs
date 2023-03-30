using System;
using System.Collections.Generic;

namespace Items
{
    public class Container
    {
        #region Fields

        private string _lock;
        private Item[] _items;

        #endregion Fields

        #region Properties

        public int Size => _items.Length;
        public bool IsLocked => _lock != string.Empty;

        #endregion Properties

        #region Constructors

        public Container(int size, string @lock)
        {
            _lock = @lock;
            _items = new Item[size];
        }

        #endregion Constructors

        #region Public

        public Item ExtractAt(int index, int count = 0)
        {
            if (_items[index] == null)
                return null;
            Item output;
            int delta = count == 0
                ? _items[index].Count
                : Math.Min(count, _items[index].Count);
            output = _items[index].Clone(delta);
            _items[index].Count -= delta;
            if (_items[index].Count == 0)
                _items[index] = null;
            return output;
        }

        public bool InsertAt(Item item, int index)
        {
            if (_items[index].Name == item.Name && _items[index].Count < item.MaxStack)
            {
                int delta = item.MaxStack - _items[index].Count;
                _items[index].Count += delta;
                item.Count -= delta;
                if (item.Count == 0)
                    return true;
                else return false;
            }
            else if (_items[index] == null)
            {
                _items[index] = item;
                return true;
            }
            return false;
        }

        public bool Insert(Item item)
        {
            List<int> itemIndexes = new();
            List<int> nullIndexes = new();
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                {
                    nullIndexes.Add(i);
                    continue;
                }
                if (_items[i].Name == item.Name && _items[i].Count < item.MaxStack)
                    itemIndexes.Add(i);
            }
            foreach (int i in itemIndexes)
            {
                int delta = item.MaxStack - _items[i].Count;
                _items[i].Count += delta;
                item.Count -= delta;
                if (item.Count == 0)
                    return true;
            }
            if (nullIndexes.Count > 0)
            {
                _items[nullIndexes[0]] = item;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            string output = string.Empty;
            foreach (var item in _items)
                output += item != null ? item + "\n" : "null\n";
            return output;
        }

        #endregion Public
    }
}