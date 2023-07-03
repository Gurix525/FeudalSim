using Cinemachine;
using Extensions;
using Input;
using UnityEngine;
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
        private float _minCameraAngle = 30F;

        [SerializeField]
        private float _maxCameraAngle = 60F;

        [SerializeField]
        private float _angleChangeScale = 10F;

        [SerializeField]
        private float _zoomChangeScale = 2F;

        [SerializeField]
        private float _rotationAdjustingSpeed = 0.1F;

        private Vector3 _targetRotation = new(60F, 0F, 0F);

        private CinemachineVirtualCamera _camera;

        private CinemachineFramingTransposer _framingTransposer;

        private bool _canSteer;

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
            PlayerController.MainMiddleMouseButton.AddListener(ActionType.Started, EnableCameraSteering);
            PlayerController.MainMiddleMouseButton.AddListener(ActionType.Canceled, DisableCameraSteering);
        }

        private void FixedUpdate()
        {
            SteerCamera();
        }

        //private void FixedUpdate()
        //{
        //    AdjustAngle();
        //}

        private void OnDisable()
        {
            PlayerController.MainMiddleMouseButton.RemoveListener(ActionType.Started, EnableCameraSteering);
            PlayerController.MainMiddleMouseButton.RemoveListener(ActionType.Canceled, DisableCameraSteering);
        }

        #endregion Unity

        #region Private

        //private void ChangeZoomParameters(InputAction.CallbackContext context)
        //{
        //    if (CursorRaycaster.IsPointerOverGameObject)
        //        return;
        //    var change = context.ReadValue<float>();
        //    if (change < 0F)
        //        _targetZoom += 2;
        //    else
        //        _targetZoom -= 2;
        //    AdjustZoom();
        //}

        private void EnableCameraSteering(InputAction.CallbackContext context)
        {
            if (CursorRaycaster.IsPointerOverGameObject)
                return;
            _canSteer = true;
        }

        private void DisableCameraSteering(InputAction.CallbackContext context)
        {
            _canSteer = false;
        }

        private void SteerCamera()
        {
            if (!_canSteer)
                return;
            Vector2 mouseDelta = PlayerController.MainMouseDelta.ReadValue<Vector2>();
            SetTargetAngle(mouseDelta.x);
            SetTargetZoom(mouseDelta.y);
            AdjustRotation();
        }

        private void SetTargetAngle(float mouseX)
        {
            Vector3 oldRotation = _targetRotation;
            float newYAngle = oldRotation.y + mouseX * Time.fixedDeltaTime * _angleChangeScale;
            _targetRotation = new(oldRotation.x, newYAngle, oldRotation.z);
        }

        private void SetTargetZoom(float mouseY)
        {
            float oldZoom = _framingTransposer.m_CameraDistance;
            float newZoom = (oldZoom - mouseY * Time.fixedDeltaTime * _zoomChangeScale).Clamp(_minZoom, _maxZoom);
            _framingTransposer.m_CameraDistance = newZoom;
            Vector3 oldRotation = _targetRotation;
            float newXAngle = (newZoom.Remap(_minZoom, _maxZoom, _minCameraAngle, _maxCameraAngle));
            _targetRotation = new(newXAngle, oldRotation.y, oldRotation.z);
        }

        private void AdjustRotation()
        {
            transform.rotation = Vector3
                .Lerp(transform.eulerAngles, _targetRotation, _rotationAdjustingSpeed)
                .ToQuaternion();
        }

        //private void AdjustZoom()
        //{
        //    _targetZoom = _targetZoom.Clamp(_minZoom, _maxZoom);
        //    _framingTransposer.m_CameraDistance = _targetZoom;

        //    _targetAngle = Mathf.Lerp(_minCameraAngle, _maxCameraAngle,
        //        _targetZoom.Remap(_minZoom, _maxZoom, 0F, 1F));
        //}

        //private void AdjustAngle()
        //{
        //    Vector3 oldRotation = transform.eulerAngles;
        //    float newAngle = Mathf.Lerp(transform.eulerAngles.x, _targetAngle, _angleAdjustingSpeed);
        //    transform.rotation = Quaternion.Euler(newAngle, oldRotation.y, oldRotation.z);
        //}

        #endregion Private
    }
}