using Items;
using UnityEngine;

namespace Controls
{
    public class CursorItemMeshHighlight : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _highlight;
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _notBlockedMaterial;
        [SerializeField] private Material _blockedMaterial;

        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private MeshCollider _collider;
        private Rigidbody _rigidbody;
        private CollisionCounter _collisionCounter;
        private Vector3 _previousPosition = Vector3.zero;
        private Mesh _previousMesh = null;
        private Material _previosMaterial = null;

        private static float _meshRotation;

        #endregion Fields

        #region Properties

        public static Vector3 Position => Instance._highlight.transform.position;
        public static Quaternion Rotation => Instance._highlight.transform.rotation;

        private static CursorItemMeshHighlight Instance { get; set; }

        public static bool IsBlocked { get; set; } = false;

        #endregion Properties

        #region Public

        public static void SetMesh(Mesh mesh)
        {
            if (Instance._mesh != mesh)
            {
                Instance._mesh = mesh;
                Instance._previousPosition = Vector3.zero;
            }
            if (mesh == null)
                Instance._renderer.enabled = false;
        }

        public static void SetMeshRotation(float rotation)
        {
            _meshRotation = rotation;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            Instance = this;
            _filter = _highlight.GetComponent<MeshFilter>();
            _renderer = _highlight.GetComponent<MeshRenderer>();
            _collider = _highlight.GetComponent<MeshCollider>();
            _rigidbody = _highlight.GetComponent<Rigidbody>();
            _collisionCounter = _highlight.GetComponent<CollisionCounter>();
            _renderer.material = _notBlockedMaterial;
        }

        private void FixedUpdate()
        {
            if (Cursor.Action is not PutAction)
            {
                SetMesh(null);
                return;
            }
            if (_mesh != _previousMesh)
            {
                _filter.mesh = _mesh;
                _collider.sharedMesh = _mesh;
                _previousMesh = _mesh;
            }
            if (_previosMaterial != _notBlockedMaterial && !IsBlocked)
            {
                _renderer.material = _notBlockedMaterial;
                _previosMaterial = _notBlockedMaterial;
            }
            else if (_previosMaterial != _blockedMaterial && IsBlocked)
            {
                _renderer.material = _blockedMaterial;
                _previosMaterial = _blockedMaterial;
            }
            if (_mesh != null && Cursor.RaycastHit != null)
            {
                _renderer.enabled = true;
                _renderer.material.renderQueue = 3001;
                var position = Cursor.RaycastHit.Value.point;
                var normal = Cursor.RaycastHit.Value.normal;
                var rotation = Quaternion.FromToRotation(Vector3.up, normal)
                    * Quaternion.Euler(0F, _meshRotation, 0F);
                _highlight.transform.SetPositionAndRotation(
                    position,
                    rotation);
                IsBlocked = _collisionCounter.IsColliding || normal.y < 0.71F;
            }
            else
                _renderer.enabled = false;
        }

        #endregion Unity
    }
}