using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controls;
using UnityEngine;
using UnityEngine.Events;
using World;
using Cursor = Controls.Cursor;
using Terrain = World.Terrain;

namespace Items
{
    public class Container : IEnumerable<Item>
    {
        #region Fields

        private string _lock;
        private Item[] _items;

        #endregion Fields

        #region Properties

        public int Size => _items.Length;
        public string Lock => _lock;
        public bool IsLocked => _lock != string.Empty;
        public bool IsArmor { get; set; }
        public IEnumerable<Item> Items => _items;

        public Item this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        public UnityEvent CollectionUpdated { get; } = new();

        #endregion Properties

        #region Constructors

        public Container(int size, string @lock = "", bool isArmor = false)
        {
            _lock = @lock;
            _items = new Item[size];
            IsArmor = isArmor;
        }

        #endregion Constructors

        #region Public

        public void SetItems(Item[] items)
        {
            _items = items;
            CollectionUpdated.Invoke();
        }

        public void ChangeSize(int newSize, Vector3 dropPosition)
        {
            if (_items.Length == newSize)
                return;
            if (_items.Length < newSize)
            {
                var list = _items.ToList();
                while (list.Count < newSize)
                    list.Add(null);
                _items = list.ToArray();
            }
            if (_items.Length > newSize)
            {
                List<Item> list = new();
                for (int i = 0; i < newSize; i++)
                    list.Add(_items[i]);
                for (int i = newSize; i < _items.Length; i++)
                {
                    DropAt(i, dropPosition);
                }
                _items = list.ToArray();
            }
        }

        public void DropAt(int index, Vector3 dropPosition, int count = 0)
        {
            Item dropItem = ExtractAt(index, count);
            if (dropItem == null)
                return;
            dropItem.Drop(dropPosition);
        }

        public void OnLeftMouseButton(int slotIndex)
        {
            if (!IsArmorMatchingSlot(Cursor.Container[0], slotIndex))
                return;
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

        //public void OnRightMouseButton(int slotIndex)
        //{
        //    if (!IsArmorMatchingSlot(Cursor.Container[0], slotIndex))
        //        return;
        //    Item thisItem = ExtractAt(slotIndex);
        //    Item cursorItem = Cursor.Container.ExtractAt(0);
        //    if (thisItem == null && cursorItem == null)
        //        return;
        //    if (thisItem != null && cursorItem == null)
        //    {
        //        int delta = thisItem.Count / 2 + thisItem.Count % 2;
        //        thisItem.Count -= delta;
        //        Cursor.Container.InsertAt(0, thisItem.Clone(delta));
        //        if (thisItem.Count == 0)
        //            thisItem = null;
        //        InsertAt(slotIndex, thisItem);
        //        CollectionUpdated.Invoke();
        //        return;
        //    }
        //    if (thisItem == null || thisItem.Name == cursorItem.Name)
        //    {
        //        var change = cursorItem.Clone(1);
        //        cursorItem.Count -= 1;
        //        if (cursorItem.Count == 0)
        //            cursorItem = null;
        //        InsertAt(slotIndex, thisItem);
        //        InsertAt(slotIndex, change);
        //        Cursor.Container.InsertAt(0, cursorItem);
        //        CollectionUpdated.Invoke();
        //        return;
        //    }
        //    InsertAt(slotIndex, thisItem);
        //    Cursor.Container.InsertAt(0, cursorItem);
        //    OnLeftMouseButton(slotIndex);
        //}

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
            else if (_items[index].Name == item.Name)
            {
                _items[index].Count += item.Count;
                item.Count = 0;
            }
            CollectionUpdated.Invoke();
        }

        public void Insert(Item item)
        {
            if (item == null)
                return;
            List<int> itemIndexes = new();
            List<int> nullIndexes = new();
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                {
                    nullIndexes.Add(i);
                    continue;
                }
                if (_items[i].Name == item.Name)
                    itemIndexes.Add(i);
            }
            foreach (int i in itemIndexes)
            {
                _items[i].Count += item.Count;
                item.Count = 0;
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

        public IEnumerator<Item> GetEnumerator()
        {
            return ((IEnumerable<Item>)_items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Public

        #region Private

        private bool IsArmorMatchingSlot(Item cursorItem, int slotIndex)
        {
            if (!IsArmor || cursorItem == null)
                return true;
            if (!cursorItem.Stats.TryGetValue("ArmorType", out string type))
                return false;
            string requiredType = slotIndex switch
            {
                0 => "Head",
                1 => "Torso",
                2 => "Legs",
                _ => "Feet"
            };
            if (type == requiredType)
                return true;
            return false;
        }

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
            return thisItem.Name == otherItem.Name;
        }

        #endregion Private
    }
}