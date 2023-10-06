using System.Collections.Generic;
using Items;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Controls
{
    public class PlayerCursor : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _canvasesParent;

        private ItemReference _itemReference;
        private LayerMask _layerMask;

        #endregion Fields

        #region Properties

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
            GetObjectUnderCursor()?
            .GetComponent<ILeftMouseButtonHandler>()?
            .OnLeftMouseButton();
        }

        private void OnRightMouseButton(InputValue value)
        {
            GetObjectUnderCursor()?
                .GetComponent<IRightMouseButtonHandler>()?
                .OnRightMouseButton();
        }

        private void OnMousePosition(InputValue value)
        {
            ScreenPosition = value.Get<Vector2>();
        }

        #endregion Input

        #region Unity

        private void Awake()
        {
            _layerMask = ~LayerMask.GetMask("Player", "Hitbox", "Attack");
        }

        private void Update()
        {
            UpdateWorldPosition();
        }

        #endregion Unity

        #region Private

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
                return hit.collider.gameObject;
            return null;
        }

        private void UpdateWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(ScreenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _layerMask))
                WorldPosition = hit.point;
        }

        #endregion Private
    }
}