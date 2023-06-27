using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combat;
using UnityEngine;

namespace Misc
{
    public class Pool<T> where T : MonoBehaviour
    {
        private LinkedList<T> _items = new();
        private GameObject _itemPrefab;
        private Transform _pool;

        private static Transform _poolsParent;

        /// <summary>
        /// Ścieżka w formacie: "Prefabs/Itp/Itd"
        /// </summary>
        public Pool(string prefabPath)
        {
            _itemPrefab = Resources.Load<GameObject>(prefabPath);
            _pool = new GameObject(nameof(T)).transform;
            _pool.SetParent(_poolsParent ??= new GameObject("Pools").transform);
        }

        public T Pull()
        {
            T item = _items.First?.Value;
            if (item != null)
                _items.RemoveFirst();
            else
                item = GameObject.Instantiate(_itemPrefab).GetComponent<T>();
            return item;
        }

        public void Push(T item)
        {
            item.transform.SetParent(_pool ??= new GameObject("ArrowsPool").transform);
            _pool.SetParent();
            item.gameObject.SetActive(false);
            _items.AddFirst(item);
        }
    }
}