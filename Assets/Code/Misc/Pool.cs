using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class Pool
    {
        private LinkedList<GameObject> _items;
        private Transform _pool;
        private GameObject _prefab;

        public Pool(GameObject prefab, Transform poolParent)
        {
            _prefab = prefab;
            _pool = new GameObject(prefab.name + "s").transform;
            _pool.SetParent(poolParent);
        }

        public GameObject Pull()
        {
            GameObject item = _items.First?.Value;
            if (item != null)
                _items.RemoveFirst();
            else
                item = GameObject.Instantiate(_prefab);
            return item;
        }

        public void Push(GameObject gameObject)
        {
            if (!_items.Contains(gameObject))
                _items.AddFirst(gameObject);
            gameObject.transform.SetParent(_pool);
            gameObject.SetActive(false);
        }
    }
}