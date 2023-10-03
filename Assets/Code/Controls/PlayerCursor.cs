using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Controls
{
    public class PlayerCursor : MonoBehaviour
    {
        #region Fields

        private static Vector2Int? _cellPosition;
        private static bool _isCombatMode;
        private static ItemReference _itemReference;

        #endregion Fields

        #region Properties

        public static bool IsAboveTerrain { get; private set; }
        public static float AlignmentMultiplier { get; set; } = 1F / 8F;
        public static UnityEvent<ItemReference> ItemReferenceChanged { get; } = new();
        public static ItemReference ItemReference
        {
            get => _itemReference;
            set
            {
                _itemReference = value;
                ItemReferenceChanged.Invoke(_itemReference);
            }
        }

        public static RaycastHit? ClearRaycastHit { get; set; }

        #endregion Properties
    }
}