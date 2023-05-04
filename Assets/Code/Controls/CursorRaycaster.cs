using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class CursorRaycaster : MonoBehaviour
    {
        [SerializeField] private float _maxCursorDistanceFromPlayer = 5F;

        private static bool _isPointerOverGameObject = false;

        public static RaycastHit? Hit { get; private set; } = null;

        public static RaycastHit? CurrentHit => GetRaycastHit();

        private static CursorRaycaster Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            _isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
            GetRaycastHit();
        }

        private static RaycastHit? GetRaycastHit()
        {
            if (_isPointerOverGameObject)
            {
                Hit = null;
                return null;
            }
            Ray ray = Camera.main
                .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
            Physics.Raycast(ray, out RaycastHit hit);
            if (Vector3.Distance(hit.point, Player.Position) > Instance._maxCursorDistanceFromPlayer)
            {
                Hit = null;
                return null;
            }
            Hit = hit;
            return hit;
        }
    }
}