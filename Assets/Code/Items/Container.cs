using System;
using System.Collections.Generic;
using System.Linq;
using Controls;
using UnityEngine.Events;

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
        public Item this[int index] => _items[index];
        public UnityEvent CollectionUpdated { get; } = new();

        #endregion Properties

        #region Constructors

        public Container(int size, string @lock = "")
        {
            _lock = @lock;
            _items = new Item[size];
        }

        #endregion Constructors

        #region Public

        public void HandleLeftClick(int slotIndex)
        {
            Item thisItem = ExtractAt(slotIndex);
            Item cursorItem = Cursor.Container.ExtractAt(0);
            if (IsPossibleToInsert(thisItem, cursorItem))
            {
                _items[slotIndex] = thisItem;
                InsertAt(slotIndex, cursorItem);
                if (cursorItem.Count != 0)
                    Cursor.Container.InsertAt(0, cursorItem);
                CollectionUpdated.Invoke();
                return;
            }
            if (cursorItem != null)
                _items[slotIndex] = cursorItem;
            if (thisItem != null)
                Cursor.Container.InsertAt(0, thisItem);
            CollectionUpdated.Invoke();
        }

        public void HandleRightClick(int slotIndex)
        {
            Item thisItem = ExtractAt(slotIndex);
            Item cursorItem = Cursor.Container.ExtractAt(0);
            if (thisItem == null && cursorItem == null)
                return;
            if (thisItem != null && cursorItem == null)
            {
                int delta = thisItem.Count / 2 + thisItem.Count % 2;
                thisItem.Count -= delta;
                Cursor.Container.InsertAt(0, thisItem.Clone(delta));
                if (thisItem.Count == 0)
                    thisItem = null;
                InsertAt(slotIndex, thisItem);
                CollectionUpdated.Invoke();
                return;
            }
            if (thisItem == null || (thisItem.Name == cursorItem.Name && thisItem.Count < thisItem.MaxStack))
            {
                var change = cursorItem.Clone(1);
                cursorItem.Count -= 1;
                if (cursorItem.Count == 0)
                    cursorItem = null;
                InsertAt(slotIndex, thisItem);
                InsertAt(slotIndex, change);
                Cursor.Container.InsertAt(0, cursorItem);
                CollectionUpdated.Invoke();
                return;
            }
            InsertAt(slotIndex, thisItem);
            Cursor.Container.InsertAt(0, cursorItem);
            HandleLeftClick(slotIndex);
        }

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
            CollectionUpdated.Invoke();
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
            CollectionUpdated.Invoke();
            return output;
        }

        public void InsertAt(int index, Item item)
        {
            if (item == null)
                return;
            if (_items[index] == null)
            {
                _items[index] = item.Clone();
                item.Count = 0;
            }
            else if (_items[index].Name == item.Name && _items[index].Count < item.MaxStack)
            {
                int delta = Math.Min(item.MaxStack - _items[index].Count, item.Count);
                _items[index].Count += delta;
                item.Count -= delta;
            }
            CollectionUpdated.Invoke();
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
                CollectionUpdated.Invoke();
                if (item.Count == 0)
                    return;
            }
            if (nullIndexes.Count > 0)
            {
                _items[nullIndexes[0]] = item.Clone();
                item.Count = 0;
            }
            CollectionUpdated.Invoke();
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

        private static bool IsPossibleToInsert(Item thisItem, Item otherItem)
        {
            if (thisItem == null || otherItem == null)
                return false;
            return thisItem.Name == otherItem.Name
                && thisItem.Count < thisItem.MaxStack;
        }

        #endregion Private
    }
}