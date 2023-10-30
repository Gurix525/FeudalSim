using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class MeshHighlight : MonoBehaviour
    {
        [SerializeField] private MainCursor _cursor;

        private MeshFilter _meshFilter;
        private PlayerInput _playerInput;
        private MeshRenderer _meshRenderer;

        private bool _isRightMouseButtonPressed;
        private Quaternion _targetRotation;
        private float _targetPivotDistance;

        private void OnRightMouseButton(InputValue value)
        {
            _isRightMouseButtonPressed = value.isPressed;
        }

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _playerInput = GetComponent<PlayerInput>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            _cursor.ItemReferenceChanged += _cursor_ItemReferenceChanged;
            _cursor.WorldPositionChanged += _cursor_WorldPositionChanged;
            _cursor.PassedRotation += _cursor_PassedRotation;
        }

        private void OnDisable()
        {
            _cursor.ItemReferenceChanged -= _cursor_ItemReferenceChanged;
            _cursor.WorldPositionChanged -= _cursor_WorldPositionChanged;
            _cursor.PassedRotation -= _cursor_PassedRotation;
        }

        private void _cursor_WorldPositionChanged(object sender, RaycastHitChangedEventArgs e)
        {
            if (_isRightMouseButtonPressed)
                return;
            if (e.NewRaycastHit != null)
            {
                transform.position = e.NewRaycastHit.Value.point;
                transform.rotation = Quaternion
                    .FromToRotation(Vector3.up, e.NewRaycastHit.Value.normal);
                //Vector3 previousCenter = _meshRenderer.bounds.center;
                //Vector3 currentCenter = _meshRenderer.bounds.center;
                //transform.position += previousCenter - currentCenter;
                transform.rotation *= _targetRotation;
            }
        }

        private void _cursor_ItemReferenceChanged(object sender, ItemReferenceChangedEventArgs e)
        {
            if (e.NewReference != null)
            {
                _meshFilter.sharedMesh = e.NewReference.Item.Mesh;
            }
            else
            {
                _targetRotation = Quaternion.identity;
                transform.rotation = Quaternion.identity;
                _meshFilter.sharedMesh = null;
            }
        }

        private void _cursor_PassedRotation(object sender, PassedRotationEventArgs e)
        {
            //Vector3 originalPosition = transform.position;
            transform.Rotate(Vector3.up, -e.Yaw);
            //transform.RotateAround(_meshRenderer.bounds.center, e.PitchAxis, Mathf.Abs(e.Pitch));
            _targetRotation = transform.rotation;
            //Vector3 changedPosition = transform.position;
            //_targetPivotDistance = _meshRenderer.bounds.extents.magnitude;
        }
    }
}