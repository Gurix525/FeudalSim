using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using Controls;
using UI;
using UnityEngine;
using UnityEngine.Events;

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

        public void DropAt(int index, Vector3 dropPosition, int count = 0, bool scatter = true)
        {
            DropAt(index, dropPosition, Quaternion.identity, count, scatter);
        }

        public void DropAt(int index, Vector3 dropPosition, Quaternion rotation, int count = 0, bool scatter = true)
        {
            Item dropItem = ExtractAt(index, count);
            if (dropItem == null)
                return;
            dropItem.Drop(dropPosition, rotation, scatter);
        }

        public void OnLeftMouseButton(int slotIndex)
        {
            if (_items[slotIndex] == null)
                return;
            MainCursor.Current.ItemReference = new(this, slotIndex);
            CollectionUpdated.Invoke();
        }

        public void OnShiftLeftMouseButton(int slotIndex)
        {
            OnLeftMouseButton(slotIndex);
        }

        public void OnLeftMouseButtonRelase(int slotIndex)
        {
            if (MainCursor.Current.ItemReference == null)
                return;
            Container other = MainCursor.Current.ItemReference.Container;
            int otherIndex = MainCursor.Current.ItemReference.Index;
            if (_items[slotIndex] == null)
                SwapItems(this, slotIndex, other, otherIndex);
            else if (_items[slotIndex].Model == other[otherIndex].Model)
                MergeItems(other, otherIndex, this, slotIndex);
            else
                SwapItems(this, slotIndex, other, otherIndex);
            MainCursor.Current.RelaseItemReference();
            CollectionUpdated.Invoke();
        }

        public void OnShiftLeftMouseButtonRelase(int slotIndex)
        {
            if (MainCursor.Current.ItemReference == null)
                return;
            Container other = MainCursor.Current.ItemReference.Container;
            int otherIndex = MainCursor.Current.ItemReference.Index;
            if (_items[slotIndex] == null)
            {
                if (other[otherIndex].Count == 1)
                    PushItemDestructive(other, otherIndex, this, slotIndex);
                else
                    QuantityMenu.Current.Show(other, otherIndex, this, slotIndex, MainCursor.Current.ScreenPosition);
            }
            else if (_items[slotIndex].Model == other[otherIndex].Model)
            {
                if (other[otherIndex].Count == 1)
                    MergeItems(other, otherIndex, this, slotIndex);
                else
                    QuantityMenu.Current.Show(other, otherIndex, this, slotIndex, MainCursor.Current.ScreenPosition);
            }
            else
                SwapItems(this, slotIndex, other, otherIndex);
            MainCursor.Current.RelaseItemReference();
            CollectionUpdated.Invoke();
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

        public void RemoveRecipeItems(Recipe recipe, bool isCheckNeccessary = false)
        {
            if (isCheckNeccessary)
            {
                if (!MatchesRecipe(recipe))
                    return;
            }
            foreach (var item in recipe.Items)
            {
                ItemModel model = Item.GetModel(item.Item.name);
                Extract(model, item.Count);
            }
        }

        public bool MatchesRecipe(Recipe recipe)
        {
            return GetRecipeMatchCount(recipe) > 0;
        }

        public int GetRecipeMatchCount(Recipe recipe)
        {
            int max = int.MaxValue;
            foreach (var item in recipe.Items)
            {
                ItemModel model = Item.GetModel(item.Item.name);
                max = Math.Min(max, GetItemCount(model) / item.Count);
            }
            return max;
        }

        public bool Contains(ItemModel model, int count)
        {
            return GetItemCount(model) >= count;
        }

        public int GetItemCount(ItemModel model)
        {
            int foundCount = 0;
            foreach (var item in _items)
            {
                if (item == null)
                    continue;
                if (item.Model == model)
                    foundCount += item.Count;
            }
            return foundCount;
        }

        public Item Extract(ItemModel model, int count = 0)
        {
            if (count == 0)
                count = int.MaxValue;
            int initialValue = count;
            for (int i = _items.Length - 1; i >= 0; i--)
            {
                if (_items[i] == null)
                    continue;
                if (_items[i].Model == model)
                {
                    int currentItemCount = _items[i].Count;
                    int greater = Math.Min(currentItemCount, count);
                    _items[i].Count -= greater;
                    count -= greater;
                    if (_items[i].Count <= 0)
                        _items[i] = null;
                    if (count <= 0)
                        break;
                }
            }
            int extractedCount = initialValue - count;
            CollectionUpdated.Invoke();
            return Item.Create(model.Name, extractedCount);
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

        public static void PushItemDestructive(Container source, int sourceIndex, Container destination, int destinationIndex, int count = 0)
        {
            Item item = source.ExtractAt(sourceIndex, count);
            destination[destinationIndex] = item;
            source.CollectionUpdated.Invoke();
            destination.CollectionUpdated.Invoke();
        }

        public static void SwapItems(Container left, int leftIndex, Container right, int rightIndex)
        {
            if (left[leftIndex] == right[rightIndex])
                return;
            Item temp = left[leftIndex];
            left[leftIndex] = right[rightIndex];
            right[rightIndex] = temp;
            left.CollectionUpdated.Invoke();
            right.CollectionUpdated.Invoke();
        }

        public static void MergeItems(Container source, int sourceIndex, Container destination, int destinationIndex, int count = 0)
        {
            if (count > source[sourceIndex].Count)
                throw new ArgumentOutOfRangeException("Count must not be greater than source count.");
            if (source[sourceIndex] == destination[destinationIndex])
                return;
            count = count == 0 ? source[sourceIndex].Count : count;
            destination[destinationIndex].Count += count;
            source[sourceIndex].Count -= count;
            if (source[sourceIndex].Count == 0)
                source[sourceIndex] = null;
            source.CollectionUpdated.Invoke();
            destination.CollectionUpdated.Invoke();
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