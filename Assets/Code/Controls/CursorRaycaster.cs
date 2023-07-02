using System;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerControls;
using System.Drawing;
using UnityEngine.ProBuilder;

namespace Controls
{
    public class CursorRaycaster : MonoBehaviour
    {
        #region Fields

        private static int _layerMask;

        #endregion Fields

        #region Properties

        public static bool IsPointerOverGameObject { get; private set; }

        public static float MaxCursorDistanceFromPlayer { get; } = 4F;

        public static RaycastHit? ClearHit { get; private set; } = null;

        public static RaycastHit? Hit { get; private set; } = null;

        public static RaycastHit? CurrentHit => GetRaycastHit();

        #endregion Properties

        #region Unity

        private void Awake()
        {
            _layerMask = ~LayerMask.GetMask("Player", "Hitbox", "Attack");
        }

        private void Update()
        {
            IsPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
            GetRaycastHit();
        }

        #endregion Unity

        #region Private

        private static RaycastHit? GetRaycastHit()
        {
            try
            {
                if (IsPointerOverGameObject)
                {
                    ClearHit = null;
                    Hit = null;
                    return null;
                }
                Ray ray = Camera.main
                    .ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
                Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, _layerMask);
                if (Vector3.Distance(hit.point, Player.Position) > MaxCursorDistanceFromPlayer
                    && Vector3.Distance(hit.collider.transform.position, Player.Position) > MaxCursorDistanceFromPlayer)
                {
                    ClearHit = hit;
                    Hit = null;
                    return null;
                }
                ClearHit = hit;
                Hit = hit;
                return hit;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static Vector3? GetPLaneHit(Vector3 normal, Vector3 point)
        {
            if (IsPointerOverGameObject)
                return null;
            var ray = Camera.main.ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
            new Plane(normal, point).Raycast(ray, out float distance);
            var position = ray.origin + ray.direction * distance;
            if ((position - Player.Position).magnitude > MaxCursorDistanceFromPlayer)
                return null;
            return position;
        }

        #endregion Private
    }
}