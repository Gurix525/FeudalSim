using System;
using Cinemachine;
using Extensions;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Controls
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class Zoom : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private float _minZoom = 4F;

        [SerializeField]
        private float _maxZoom = 20F;

        [SerializeField]
        private float _targetZoom = 16F;

        [SerializeField]
        private float _minCameraAngle = 30F;

        [SerializeField]
        private float _maxCameraAngle = 60F;

        [SerializeField]
        private float _angleCorrectingSpeed = 0.1F;

        private float currentAngle = 60F;

        private float _targetAngle = 60F;

        private CinemachineVirtualCamera _camera;

        private CinemachineFramingTransposer _framingTransposer;

        #endregion Fields

        #region Unity

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _framingTransposer = _camera
                .GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void OnEnable()
        {
            PlayerController.MainScroll.AddListener(ActionType.Started, DoZoom);
        }

        private void FixedUpdate()
        {
            Vector3 oldRotation = transform.eulerAngles;
            float newAngle = Mathf.Lerp(transform.eulerAngles.x, _targetAngle, _angleCorrectingSpeed);
            transform.rotation = Quaternion.Euler(newAngle, oldRotation.y, oldRotation.z);
        }

        private void OnDisable()
        {
            PlayerController.MainScroll.RemoveListener(ActionType.Started, DoZoom);
        }

        #endregion Unity

        #region Private

        private void DoZoom(InputAction.CallbackContext obj)
        {
            if (CursorRaycaster.IsPointerOverGameObject)
                return;
            var change = PlayerController.MainScroll.ReadValue<float>();
            if (change < 0F)
                _targetZoom += 2;
            else
                _targetZoom -= 2;
            _targetZoom = _targetZoom.Clamp(_minZoom, _maxZoom);
            _framingTransposer.m_CameraDistance = _targetZoom;

            _targetAngle = Mathf.Lerp(_minCameraAngle, _maxCameraAngle,
                _targetZoom.Remap(_minZoom, _maxZoom, 0F, 1F));
        }

        #endregion Private
    }
}