using System;
using Controls;
using UnityEngine;

namespace Buildings
{
    public class Door : MonoBehaviour, IMouseHandler
    {
        private float _originalRotation = 0F;
        private float _targetRotation = 0F;
        private bool _isInitialized = false;

        public void OnLeftMouseButton(Vector2 position)
        {
            if (!_isInitialized)
            {
                _originalRotation = transform.rotation.y;
                _isInitialized = true;
            }
            _targetRotation = Math.Abs(_targetRotation - 90F);
            transform.rotation = Quaternion.Euler(0F, _originalRotation + _targetRotation, 0F);
        }
    }
}