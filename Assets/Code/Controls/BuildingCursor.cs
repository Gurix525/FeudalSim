using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class BuildingCursor : MonoBehaviour
    {
        #region Events

        public event EventHandler<ObjectChangedEventArgs> BuildingPrefabChanged;

        public event EventHandler<bool> DestroyingModeChanged;

        #endregion Events

        #region Fields

        [SerializeField] private MainCursor _mainCursor;
        //[SerializeField] private GameObject _meshHighlight;

        private GameObject _buildingPrefab;
        private RaycastHit _cursorRaycastHit;
        private bool _isOverUI;
        private bool _isDestroying;

        #endregion Fields

        #region Properties

        public GameObject BuildingPrefab
        {
            get => _buildingPrefab;
            set
            {
                if (value != _buildingPrefab)
                {
                    ObjectChangedEventArgs eventArgs = new(_buildingPrefab, value);
                    _buildingPrefab = value;
                    BuildingPrefabChanged?.Invoke(this, eventArgs);
                }
            }
        }

        public bool IsDestroying
        {
            get => _isDestroying;
            set
            {
                if (value != _isDestroying)
                {
                    _isDestroying = value;
                    DestroyingModeChanged?.Invoke(this, value);
                }
            }
        }

        public static BuildingCursor Current { get; private set; }

        #endregion Properties

        #region Input

        private void OnLeftMouseButton(InputValue value)
        {
            if (value.isPressed)
                OnLeftMouseButtonPress();
            else
                OnLeftMouseButtonRelase();
        }

        #endregion Input

        #region Unity

        private void Awake()
        {
            Current = this;
            _mainCursor.WorldPositionChanged += _mainCursor_WorldPositionChanged;
            _mainCursor.OverUIStateChanged += _mainCursor_OverUIStateChanged;
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            BuildingPrefab = null;
            IsDestroying = false;
        }

        #endregion Unity

        #region Private

        private void _mainCursor_OverUIStateChanged(object sender, bool e)
        {
            _isOverUI = e;
            if (_isOverUI)
                OnLeftMouseButtonRelase();
        }

        private void _mainCursor_WorldPositionChanged(object sender, RaycastHitChangedEventArgs e)
        {
            _cursorRaycastHit = e.NewRaycastHit.Value;
        }

        private void OnLeftMouseButtonPress()
        {
        }

        private void OnLeftMouseButtonRelase()
        {
        }

        #endregion Private
    }
}