using System;
using Misc;
using UnityEngine;
using UnityEngine.UI;
using Input;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;

namespace Items
{
    [RequireComponent(typeof(Outline))]
    public class ContainerHandler : MonoBehaviour
    {
        #region Fields

        private int _size = 4;
        private string _lock = string.Empty;
        private Container _container;
        private GameObject _window;
        private GameObject[] _slots;
        private Outline _outline;

        #endregion Fields

        #region Public

        public void ShowContainer(CallbackContext context)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _window.SetActive(true);
                PlayerController.MainEscape.AddListener(ActionType.Started, HideContainer);
            }
        }

        public void HideContainer(CallbackContext context)
        {
            PlayerController.MainEscape.RemoveListener(ActionType.Started, HideContainer);
            _window.SetActive(false);
        }

        #endregion Public

        #region Unity

        private void Start()
        {
            _container = new(_size, _lock);
            _outline = GetComponent<Outline>();
            _outline.OutlineColor = new(0F, 0F, 0F, 0F);
            _window = Instantiate(
                Prefabs.GetPrefab("ContainerWindow"),
                References.GetReference("Canvas").transform);
            var windowTransform = _window.GetComponent<RectTransform>();
            float newSize = windowTransform.sizeDelta.x * (float)Math.Sqrt(_size) - 8F;
            windowTransform.sizeDelta = new(newSize, newSize);
            var windowGridLayout = _window.GetComponent<GridLayoutGroup>();
            windowGridLayout.constraintCount = (int)Math.Sqrt(_size);
            _window.SetActive(false);
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

        private void OnMouseEnter()
        {
            PlayerController.MainRightClick.AddListener(ActionType.Started, ShowContainer);
        }

        private void OnMouseOver()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                _outline.OutlineColor = new(0.8F, 0.8F, 1F, 1F);
        }

        private void OnMouseExit()
        {
            PlayerController.MainRightClick.RemoveListener(ActionType.Started, ShowContainer);
            _outline.OutlineColor = new(0F, 0F, 0F, 0F);
        }

        private void StartTest()
        {
            Debug.Log("TESTOWA METODA");
            _container.Insert(Item.Create("Stone", 7));
            _container.Insert(Item.Create("Wood", 5));
            _container.Insert(Item.Create("Sword", 1));
            _container.Insert(Item.Create("Axe", 1));
        }

        #endregion Unity
    }
}