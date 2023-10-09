using System;
using Misc;
using UnityEngine;
using UnityEngine.UI;
 
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UI;
using Controls;
using Cursor = Controls.PlayerCursor;
using PlayerControls;

namespace Items
{
    [RequireComponent(typeof(OutlineHandler))]
    public class ContainerHandler : MonoBehaviour, IMouseHandler
    {
        #region Fields

        private int _size = 9;
        private string _lock = string.Empty;
        private Container _container;
        private GameObject _window;
        private GameObject[] _slots;
        private RectTransform _windowTransform;
        private bool _isPointerOverGameObject;

        #endregion Fields

        #region Public

        public void OnLeftMouseButton(Vector2 position)
        {
            SwitchContainerState();
        }

        #endregion Public

        #region Unity

        private void Start()
        {
            _container = new(_size, _lock);
            _window = Instantiate(
                Resources.Load<GameObject>("Prefabs/UI/ContainerWindow"),
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
                    Resources.Load<GameObject>("Prefabs/UI/ContainerSlot"),
                    _window.transform);
                var containerSlot = _slots[i].GetComponent<ContainerSlot>();
                containerSlot.Initialize(i, _container);
            }
            StartTest();
        }

        //private void FixedUpdate()
        //{
        //    if (Vector3.Distance(Player.Position, transform.position)
        //        > CursorRaycaster.MaxCursorDistanceFromPlayer)
        //        HideContainer(new());
        //}

        //private void OnMouseOver()
        //{
        //    _isPointerOverGameObject = CursorRaycaster.IsPointerOverGameObject;
        //}

        private void OnDestroy()
        {
            Destroy(_window);
        }

        private void StartTest()
        {
            Debug.Log("TESTOWA METODA");
            _container.Insert(Item.Create("Stone", 10));
            _container.Insert(Item.Create("Bow"));
            _container.Insert(Item.Create("Wood", 20));
            _container.Insert(Item.Create("Pickaxe"));
            _container.Insert(Item.Create("Sword"));
            _container.Insert(Item.Create("Axe"));
            _container.Insert(Item.Create("Shovel"));
            _container.Insert(Item.Create("Workbench"));
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
            if (!_isPointerOverGameObject)
            {
                _window.SetActive(true);
                // To be added
                //PlayerController.MainEscape.AddListener(ActionType.Started, HideContainer);
                //PlayerController.MainTab.AddListener(ActionType.Started, HideContainer);
            }
        }

        private void HideContainer(CallbackContext context)
        {
            if (context.action != null)
                if (context.action.ToString() == "Main/Tab[/Keyboard/tab]"
                    && Equipment.IsVisible)
                    return;
            // To be added
            //PlayerController.MainEscape.RemoveListener(ActionType.Started, HideContainer);
            //PlayerController.MainTab.RemoveListener(ActionType.Started, HideContainer);
            _window.SetActive(false);
        }

        #endregion Private
    }
}