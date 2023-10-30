using System;
using Buildings;
using Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class CombatCursor : MonoBehaviour
    {
        #region Fields

        [SerializeField] private MainCursor _mainCursor;

        private Vector3 _cursorWorldPosition;
        private bool _isOverUI;

        #endregion Fields

        #region Properties

        public static CombatCursor Current { get; private set; }

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
            _mainCursor.OverUIStateChanged += _mainCursor_OverUIStateChanged;
            gameObject.SetActive(false);
        }

        #endregion Unity

        #region Private

        private void _mainCursor_OverUIStateChanged(object sender, bool e)
        {
            _isOverUI = e;
            if (_isOverUI)
                OnLeftMouseButtonRelase();
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