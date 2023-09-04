using AI;
using Extensions;
using Input;
using Items;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace Controls
{
    public static class Cursor
    {
        #region Fields

        private static Vector2Int? _cellPosition;
        private static bool _isCombatMode;

        #endregion Fields

        #region Properties

        public static bool IsCombatMode
        {
            get => _isCombatMode;
            set
            {
                _isCombatMode = value;
                CombatModeSwitched.Invoke(value);
            }
        }

        public static bool IsAboveTerrain { get; private set; }
        public static float AlignmentMultiplier { get; set; } = 1F / 8F;
        public static Container Container { get; } = new(1);
        public static UnityEvent<bool> CombatModeSwitched { get; } = new();

        public static Vector2Int? CellPosition
        {
            get => _cellPosition;
            set
            {
                _cellPosition = value;
                IsAboveTerrain = _cellPosition != null ? true : false;
            }
        }

        public static Item Item =>
            Container[0];// ?? HotbarItem;

        /// <summary>
        /// Do użycia w normalnych warunkach, jeśli potrzeba rzucić raycast
        /// w danej chwili to użyć CurrentRaycastHit
        /// </summary>
        public static RaycastHit? RaycastHit => IsCombatMode
            ? null
            : CursorRaycaster.Hit;

        public static RaycastHit? ClearRaycastHit =>
            CursorRaycaster.ClearHit;

        /// <summary>
        /// Do użycia jeśli RaycastHit przekazuje nieaktualną wartość
        /// (np. kiedy event OnMouseEnter potrzebuje aktualnego hita)
        /// </summary>
        public static RaycastHit? CurrentRaycastHit => IsCombatMode
            ? null
            : CursorRaycaster.CurrentHit;

        #endregion Properties

        #region Public

        public static Vector3? GetPlaneHit(Vector3 normal, Vector3 point)
        {
            return CursorRaycaster.GetPLaneHit(normal, point);
        }

        public static Vector3 GetAlignedPosition()
        {
            Vector3 position = RaycastHit == null ? Vector3.zero : RaycastHit.Value.point;
            if (AlignmentMultiplier < 1F / 4F)
                return position;
            Vector3 positionRounded = position.Round(AlignmentMultiplier);
            return new Vector3(positionRounded.x, position.y, positionRounded.z);
        }

        public static void OnLeftMouseButton()
        {
            if (CurrentRaycastHit == null)
                return;
            RaycastHit.Value.collider.TryGetComponent(out ILeftClickHandler handler);
            if (handler != null)
                handler.OnLeftMouseButton();
        }

        #endregion Public
    }
}