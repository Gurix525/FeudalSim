using System;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ContainerHandler : MonoBehaviour
    {
        #region Fields

        private int _size = 4;
        private string _lock = string.Empty;
        private Container _container;
        private GameObject _window;
        private GameObject[] _slots;

        #endregion Fields

        #region Public

        public void ShowContainer()
        {
            _window.SetActive(true);
        }

        public void HideContainer()
        {
            _window.SetActive(false);
        }

        #endregion Public

        #region Unity

        private void Start()
        {
            _container = new(_size, _lock);
            _window = Instantiate(
                Prefabs.GetPrefab("ContainerWindow"),
                References.GetReference("Canvas").transform);
            var windowTransform = _window.GetComponent<RectTransform>();
            float newSize = windowTransform.sizeDelta.x * (float)Math.Sqrt(_size) - 8F;
            windowTransform.sizeDelta = new(newSize, newSize);
            var windowGridLayout = _window.GetComponent<GridLayoutGroup>();
            windowGridLayout.constraintCount = (int)Math.Sqrt(_size);
            //_window.SetActive(false);
            _slots = new GameObject[_size];
            for (int i = 0; i < _size; i++)
            {
                _slots[i] = Instantiate(
                    Prefabs.GetPrefab("ContainerSlot"),
                    _window.transform);
                var containerSlot = _slots[i].GetComponent<ContainerSlot>();
                containerSlot.Initialize(i, _container);
            }
            StartTest();
        }

        private void StartTest()
        {
            Debug.Log("TESTOWA METODA");
            _container.Insert(Item.Create("Stone", 7));
            _container.Insert(Item.Create("Wood", 7));
            _container.Insert(Item.Create("Sword", 1));
            _container.Insert(Item.Create("Axe", 1));
        }

        #endregion Unity
    }
}