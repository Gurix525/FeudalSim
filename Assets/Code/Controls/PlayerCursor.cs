using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.XR.OpenVR;

namespace Controls
{
    public class PlayerCursor : MonoBehaviour
    {
        #region Events

        public event EventHandler<ObjectChangedEventArgs> ObjectUnderCursorChanged;

        #endregion Events

        #region Fields

        [SerializeField] private GameObject _canvasesParent;

        private ItemReference _itemReference;
        private LayerMask _layerMask;
        private GameObject _objectUnderCursor;
        private GameObject _draggedObject;

        #endregion Fields

        #region Properties

        public static PlayerCursor Current { get; private set; }

        public GameObject ObjectUnderCursor
        {
            get => _objectUnderCursor;
            set
            {
                if (_objectUnderCursor != value)
                {
                    ObjectChangedEventArgs args = new(_objectUnderCursor, value);
                    _objectUnderCursor = value;
                    ObjectUnderCursorChanged?.Invoke(this, args);
                }
            }
        }

        public Vector3 WorldPosition { get; private set; }
        public Vector2 ScreenPosition { get; private set; }

        public ItemReference ItemReference
        {
            get => _itemReference;
            set
            {
                _itemReference = value;
                ItemReferenceChanged.Invoke(_itemReference);
            }
        }

        public UnityEvent<ItemReference> ItemReferenceChanged { get; } = new();

        #endregion Properties

        #region Input

        private void OnLeftMouseButton(InputValue value)
        {
            if (value.isPressed)
            {
                ObjectUnderCursor?
                    .GetComponents<IMouseHandler>()
                    .ToList()
                    .ForEach(handler => handler.OnLeftMouseButton(ScreenPosition));
                SetDraggedObject(ObjectUnderCursor);
            }
            else
            {
                ObjectUnderCursor?
                    .GetComponent<IMouseHandler>()?
                    .OnLeftMouseButtonRelase();
                RelaseItemReference();
                SetDraggedObject(null);
            }
        }

        private void OnRightMouseButton(InputValue value)
        {
            if (value.isPressed)
            {
                ObjectUnderCursor?
                    .GetComponent<IMouseHandler>()?
                    .OnRightMouseButton();
            }
            else
            {
                ObjectUnderCursor?
                .GetComponent<IMouseHandler>()?
                .OnRightMouseButtonRelase();
            }
        }

        private void OnMousePosition(InputValue value)
        {
            ScreenPosition = value.Get<Vector2>();
            _draggedObject?
                .GetComponents<IMouseHandler>().ToList()
                .ForEach(handler => handler.OnMousePosition(ScreenPosition));
        }

        private void OnMouseDelta(InputValue value)
        {
            _draggedObject?
                .GetComponents<IMouseHandler>().ToList()
                .ForEach(handler => handler.OnMouseDelta(value.Get<Vector2>()));
        }

        #endregion Input

        #region Unity

        private void Awake()
        {
            _layerMask = ~LayerMask.GetMask("Player", "Hitbox", "Attack");
            ObjectUnderCursorChanged += PlayerCursor_ObjectUnderCursorChanged;
            Current = this;
        }

        private void Update()
        {
            UpdateWorldPosition();
            UpdateObjectUnderCursor();
        }

        #endregion Unity

        #region Private

        private void UpdateWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(ScreenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _layerMask))
                WorldPosition = hit.point;
        }

        private void UpdateObjectUnderCursor()
        {
            ObjectUnderCursor = GetObjectUnderCursor();
        }

        private GameObject GetObjectUnderCursor()
        {
            List<RaycastResult> results = new();
            List<Transform> children = new();
            foreach (Transform child in _canvasesParent.transform)
                if (child.gameObject.activeInHierarchy)
                    children.Add(child);
            children.Reverse();
            foreach (Transform child in children)
            {
                PointerEventData pointerEventData = new(EventSystem.current);
                pointerEventData.pressPosition = ScreenPosition;
                pointerEventData.position = ScreenPosition;
                child.GetComponent<GraphicRaycaster>()
                    .Raycast(pointerEventData, results);
                if (results.Count > 0)
                    break;
            }
            if (results.Count > 0)
            {
                return results[0].gameObject;
            }
            Ray ray = Camera.main.ScreenPointToRay(ScreenPosition);
            Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _layerMask);
            if (hit.collider != null)
                return hit.collider?.gameObject;
            return null;
        }

        private void PlayerCursor_ObjectUnderCursorChanged(object sender, ObjectChangedEventArgs e)
        {
            if (e == null)
                return;
            if (e.PreviousObject != null)
                e.PreviousObject.GetComponent<IMouseHandler>()?.OnHoverEnd();
            if (e.NewObject != null)
                e.NewObject.GetComponent<IMouseHandler>()?.OnHoverStart();
        }
        private void SetDraggedObject(GameObject value)
        {
            if (value != null)
                if (value.TryGetComponent(out IMouseHandler handler))
                {
                    _draggedObject = gameObject;
                    return;
                }
            _draggedObject = null;
        }

        private void RelaseItemReference()
        {
            ItemReference = null;
        }

        #endregion Private

        public class ObjectChangedEventArgs : EventArgs
        {
            public GameObject PreviousObject { get; }
            public GameObject NewObject { get; }

            public ObjectChangedEventArgs(GameObject previousObject, GameObject newObject)
            {
                PreviousObject = previousObject;
                NewObject = newObject;
            }
        }
    }
}