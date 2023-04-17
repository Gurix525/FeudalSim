using System;
using Misc;
using UnityEngine;
using UnityEngine.UI;
using Input;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UI;
using Controls;
using Cursor = Controls.Cursor;

namespace Items
{
    [RequireComponent(typeof(OutlineHandler))]
    public class ContainerHandler : MonoBehaviour, IRightClickHandler, INoActionOutline
    {
        #region Fields

        private int _size = 9;
        private string _lock = string.Empty;
        private Container _container;
        private GameObject _window;
        private GameObject[] _slots;
        private RectTransform _windowTransform;
        private OutlineHandler _outlineHandler;

        #endregion Fields

        #region Public

        public void OnRightMouseButton()
        {
            SwitchContainerState();
        }

        public void EnableOutline()
        {
            _outlineHandler.EnableOutline();
        }

        public void DisableOutline()
        {
            _outlineHandler.DisableOutline();
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _outlineHandler = GetComponent<OutlineHandler>();
        }

        private void Start()
        {
            _container = new(_size, _lock);
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

        private void OnMouseOver()
        {
            Cursor.Action.OnMouseOver(this);
        }

        private void OnMouseExit()
        {
            Cursor.Action.OnMouseExit(this);
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

        private void SwitchContainerState()
        {
            if (_window.activeInHierarchy)
                HideContainer(new());
            else
                ShowContainer();
        }

        private void ShowContainer()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _window.SetActive(true);
                PlayerController.MainEscape.AddListener(ActionType.Started, HideContainer);
                PlayerController.MainTab.AddListener(ActionType.Started, HideContainer);
            }
        }

        private void HideContainer(CallbackContext context)
        {
            PlayerController.MainEscape.RemoveListener(ActionType.Started, HideContainer);
            PlayerController.MainTab.RemoveListener(ActionType.Started, HideContainer);
            _window.SetActive(false);
        }

        #endregion Private
    }
}