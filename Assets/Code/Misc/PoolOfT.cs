﻿using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class Pool<T> where T : MonoBehaviour
    {
        private LinkedList<T> _items = new();
        private GameObject _itemPrefab;
        private Transform _pool;
        private Transform _parent;
        private string _prefabPath;

        private static Transform _poolsParent;

        /// <summary>
        /// Ścieżka w formacie: "Prefabs/Itp/Itd"
        /// </summary>
        public Pool(string prefabPath, Transform parent = null)
        {
            _prefabPath = prefabPath;
            _parent = parent;
        }

        public Pool(GameObject itemPrefab, Transform parent = null)
        {
            _itemPrefab = itemPrefab;
            _parent = parent;
        }

        public T Pull()
        {
            T item = _items.First?.Value;
            if (item != null)
                _items.RemoveFirst();
            else
                item = GameObject
                    .Instantiate(_itemPrefab ??= Resources.Load<GameObject>(_prefabPath))
                    .GetComponent<T>();
            return item;
        }

        public void Push(T item)
        {
            item.transform.SetParent(_pool ??= new GameObject(typeof(T).Name + "s").transform);
            _pool.SetParent(_parent ?? (_poolsParent ??= new GameObject("Pools").transform));
            item.gameObject.SetActive(false);
            if (!_items.Contains(item))
                _items.AddFirst(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }
    }
}