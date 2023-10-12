using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Controls
{
    public partial class PlayerCursor : MonoBehaviour
    {
        #region Events

        public event EventHandler<ObjectChangedEventArgs> ObjectUnderCursorChanged;

        public event EventHandler<ItemReferenceChangedEventArgs> ItemReferenceChanged;

        public event EventHandler<RaycastHitChangedEventArgs> WorldPositionChanged;

        #endregion Events

        #region Fields

        [SerializeField] private GameObject _canvasesParent;
        [SerializeField] private MeshHighlight _meshHighlight;

        private ItemReference _itemReference;
        private GameObject _objectUnderCursor;
        private GameObject _draggedObject;
        private LayerMask _layerMask;
        private RaycastHit _worldRaycastHit;
        private bool _isShiftPressed;

        #endregion Fields

        #region Properties

        public static PlayerCursor Current { get; private set; }

        public Vector2 ScreenPosition { get; private set; }

        public RaycastHit WorldRaycastHit
        {
            get => _worldRaycastHit;
            private set
            {
                if (_worldRaycastHit.point != value.point)
                {
                    RaycastHitChangedEventArgs args = new(_worldRaycastHit, value);
                    _worldRaycastHit = value;
                    WorldPositionChanged?.Invoke(this, args);
                }
            }
        }

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

        public ItemReference ItemReference
        {
            get => _itemReference;
            set
            {
                if (_itemReference != value)
                {
                    ItemReferenceChangedEventArgs args = new(_itemReference, value);
                    _itemReference = value;
                    ItemReferenceChanged?.Invoke(this, args);
                }
            }
        }

        #endregion Properties

        #region Public

        public void RelaseItemReference()
        {
            ItemReference = null;
        }

        #endregion Public

        #region Input

        private void OnLeftMouseButton(InputValue value)
        {
            // On pressed
            if (value.isPressed)
            {
                if (ObjectUnderCursor)
                    ObjectUnderCursor
                        .GetComponents<IMouseHandler>()
                        .ToList()
                        .ForEach(handler =>
                        {
                            switch (_isShiftPressed)
                            {
                                case true:
                                    handler.OnShiftLeftMouseButton(ScreenPosition);
                                    break;

                                default:
                                    handler.OnLeftMouseButton(ScreenPosition);
                                    break;
                            }
                        });
                SetDraggedObject(ObjectUnderCursor);
            }
            // On relased
            else
            {
                if (ObjectUnderCursor)
                {
                    ObjectUnderCursor
                        .GetComponents<IMouseHandler>()
                        .ToList()
                        .ForEach(handler =>
                        {
                            switch (_isShiftPressed)
                            {
                                case true:
                                    handler.OnShiftLeftMouseButtonRelase();
                                    break;

                                default:
                                    handler.OnLeftMouseButtonRelase();
                                    break;
                            }
                        });
                    if (ItemReference != null && !ObjectUnderCursor
                            .TryGetComponent(out RectTransform rectTransform))
                        PutItem();
                }
                RelaseItemReference();
                SetDraggedObject(null);
            }
        }

        private void OnRightMouseButton(InputValue value)
        {
            // On pressed
            if (value.isPressed)
            {
                if (ObjectUnderCursor)
                    ObjectUnderCursor
                        .GetComponents<IMouseHandler>()
                        .ToList()
                        .ForEach(handler => handler.OnRightMouseButton());
            }
            // On relased
            else
            {
                if (ObjectUnderCursor)
                    ObjectUnderCursor
                        .GetComponents<IMouseHandler>()
                        .ToList()
                        .ForEach(handler => handler.OnRightMouseButtonRelase());
            }
        }

        private void OnMousePosition(InputValue value)
        {
            ScreenPosition = value.Get<Vector2>();
            if (_draggedObject)
                _draggedObject
                    .GetComponents<IMouseHandler>().ToList()
                    .ForEach(handler => handler.OnMousePosition(ScreenPosition));
        }

        private void OnMouseDelta(InputValue value)
        {
            Vector2 delta = value.Get<Vector2>();
            if (_draggedObject)
                _draggedObject
                    .GetComponents<IMouseHandler>().ToList()
                    .ForEach(handler => handler.OnMouseDelta(delta));
        }

        private void OnShift(InputValue value)
        {
            _isShiftPressed = value.isPressed;
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
            {
                WorldRaycastHit = hit;
            }
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
            if (e.PreviousObject)
                e.PreviousObject
                    .GetComponents<IMouseHandler>()
                    .ToList()
                    .ForEach(handler => handler.OnHoverEnd());
            if (e.NewObject)
                e.NewObject
                    .GetComponents<IMouseHandler>()
                    .ToList()
                    .ForEach(handler => handler.OnHoverStart());
        }

        private void SetDraggedObject(GameObject value)
        {
            _draggedObject = value;
            //if (value != null)
            //    if (value.TryGetComponent(out IMouseHandler handler))
            //    {
            //        _draggedObject = gameObject;
            //        return;
            //    }
            //_draggedObject = null;
        }

        private void PutItem()
        {
            if (ItemReference.Item.Mesh != null)
                ItemReference.Container.DropAt(
                    ItemReference.Index,
                    WorldRaycastHit.point,
                    _meshHighlight.transform.rotation,
                    scatter: false);
        }

        #endregion Private
    }
}