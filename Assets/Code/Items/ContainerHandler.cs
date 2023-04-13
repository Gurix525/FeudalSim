using System;
using Misc;
using UnityEngine;
using UnityEngine.UI;
using Input;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UI;

namespace Items
{
    [RequireComponent(typeof(Outline))]
    public class ContainerHandler : MonoBehaviour
    {
        #region Fields

        private int _size = 9;
        private string _lock = string.Empty;
        private Container _container;
        private GameObject _window;
        private GameObject[] _slots;
        private Outline _outline;
        private RectTransform _windowTransform;

        private bool _isOutlineActive = false;

        #endregion Fields

        #region Unity

        private void Start()
        {
            _container = new(_size, _lock);
            _outline = GetComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineColor = new(0F, 0F, 0F, 0F);
            _outline.OutlineWidth = 1F;
            _window = Instantiate(
                Prefabs.GetPrefab("ContainerWindow"),
                References.GetReference("DefaultCanvas").transform);
            _windowTransform = _window.GetComponent<RectTransform>();
            float newSize = _windowTransform.sizeDelta.x * (float)Math.Sqrt(_size) - 8F;
            _windowTransform.sizeDelta = new(newSize, newSize);
            var windowGridLayout = _window.GetComponent<GridLayoutGroup>();
            windowGridLayout.constraintCount = (int)Math.Sqrt(_size);
            _window.GetComponent<ContainerWindow>().Initialize(_container, this);
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
            PlayerController.MainRightClick.AddListener(ActionType.Started, SwitchContainer);
        }

        private void OnMouseOver()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (!_isOutlineActive)
                {
                    _outline.OutlineColor = new(1F, 3F, 2F, 1F);
                    _isOutlineActive = true;
                }
            }
        }

        private void OnMouseExit()
        {
            PlayerController.MainRightClick.RemoveListener(ActionType.Started, SwitchContainer);
            _outline.OutlineColor = new(0F, 0F, 0F, 0F);
            _isOutlineActive = false;
        }

        private void OnDestroy()
        {
            Destroy(_window);
        }

        private void StartTest()
        {
            Debug.Log("TESTOWA METODA");
            _container.Insert(Item.Create("Stone", 10));
            _container.Insert(Item.Create("Stone", 10));
            _container.Insert(Item.Create("Wood", 20));
            _container.Insert(Item.Create("Wood", 20));
            _container.Insert(Item.Create("Wood", 20));
            _container.Insert(Item.Create("Axe"));
            _container.Insert(Item.Create("Shovel"));
            _container.Insert(Item.Create("Sword"));
        }

        #endregion Unity

        #region Private

        private void SwitchContainer(CallbackContext context)
        {
            if (_window.activeInHierarchy)
                HideContainer(new());
            else
                ShowContainer(new());
        }

        private void ShowContainer(CallbackContext context)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _window.SetActive(true);
                PlayerController.MainEscape.AddListener(ActionType.Started, HideContainer);
            }
        }

        private void HideContainer(CallbackContext context)
        {
            PlayerController.MainEscape.RemoveListener(ActionType.Started, HideContainer);
            _window.SetActive(false);
        }

        #endregion Private
    }
}