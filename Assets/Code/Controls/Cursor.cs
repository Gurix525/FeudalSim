using Items;
using Misc;
using UnityEngine;

namespace Controls
{
    public static class Cursor
    {
        private static Vector2Int? _cellPosition;

        public static Container Container { get; } = new(1);

        public static Item Item => Container[0];

        public static Vector2Int? CellPosition
        {
            get => _cellPosition;
            set
            {
                _cellPosition = value;
                IsAboveTerrain = _cellPosition != null ? true : false;
            }
        }

        public static bool IsAboveTerrain { get; private set; } = false;

        public static ItemAction Action => Item?.Action ?? ItemAction.NoAction;

        /// <summary>
        /// Do użycia w normalnych warunkach, jeśli potrzeba rzucić raycast
        /// w danej chwili to użyć CurrentRaycastHit
        /// </summary>
        public static RaycastHit? RaycastHit => CursorRaycaster.Hit;

        /// <summary>
        /// Do użycia jeśli RaycastHit przekazuje nieaktualną wartość
        /// (np. kiedy event OnMouseEnter potrzebuje aktualnego hita)
        /// </summary>
        public static RaycastHit? CurrentRaycastHit => CursorRaycaster.CurrentHit;
    }
}