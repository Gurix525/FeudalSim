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
        [SerializeField]
        private float _minZoom = 4F;

        [SerializeField]
        private float _maxZoom = 20F;

        [SerializeField]
        private float _targetZoom = 16F;

        private CinemachineVirtualCamera _camera;

        private CinemachineFramingTransposer _framingTransposer;

        private CinemachinePOV _pov;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _framingTransposer = _camera
                .GetCinemachineComponent<CinemachineFramingTransposer>();
            _pov = _camera.GetCinemachineComponent<CinemachinePOV>();
        }

        private void OnEnable()
        {
            PlayerController.MainScroll.AddListener(ActionType.Started, DoZoom);
        }

        private void OnDisable()
        {
            PlayerController.MainScroll.RemoveListener(ActionType.Started, DoZoom);
        }

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

            //_pov.m_VerticalAxis.Value =
            //    Mathf.Lerp(30F, 60F, _targetZoom.Remap(_minZoom, _maxZoom, 0F, 1F));
        }
    }
}