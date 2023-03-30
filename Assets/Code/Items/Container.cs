﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        public Container(int size, string @lock = "")
        {
            _lock = @lock;
            _items = new Item[size];
        }

        #endregion Constructors

        #region Public

        public void Sort(bool hasToStack = true)
        {
            if (hasToStack)
                StackItems();
            var sortedItems = _items
                .Where(item => item != null)
                .OrderBy(item => item.Name)
                .ThenByDescending(item => item.Count)
                .ToArray();
            Array.Clear(_items, 0, _items.Length);
            for (int i = 0; i < sortedItems.Length; i++)
                _items[i] = sortedItems[i];
        }

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

        public void InsertAt(int index, Item item)
        {
            if (_items[index] == null)
            {
                _items[index] = item.Clone();
                item.Count = 0;
            }
            else if (_items[index].Name == item.Name && _items[index].Count < item.MaxStack)
            {
                int delta = item.MaxStack - _items[index].Count;
                _items[index].Count += delta;
                item.Count -= delta;
            }
        }

        public void Insert(Item item)
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
                int delta = Math.Min(item.Count, item.MaxStack - _items[i].Count);
                _items[i].Count += delta;
                item.Count -= delta;
                if (item.Count == 0)
                    return;
            }
            if (nullIndexes.Count > 0)
            {
                _items[nullIndexes[0]] = item.Clone();
                item.Count = 0;
            }
        }

        public override string ToString()
        {
            string output = string.Empty;
            foreach (var item in _items)
                output += item != null ? item + "\n" : "null\n";
            return output;
        }

        #endregion Public

        #region Private

        private void StackItems()
        {
            for (int i = 0; i < _items.Length - 1; i++)
            {
                if (_items[i] == null)
                    continue;
                for (int j = i + 1; j < _items.Length; j++)
                {
                    InsertAt(j, _items[i]);
                    if (_items[i].Count == 0)
                    {
                        _items[i] = null;
                        break;
                    }
                }
            }
        }

        #endregion Private
    }
}