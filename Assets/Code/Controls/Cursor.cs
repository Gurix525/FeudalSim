using Items;
using UnityEngine;

namespace Controls
{
    public static class Cursor
    {
        private static Vector2Int? _cellPosition;

        public static Container Container { get; } = new(1);

        public static Vector2Int? CellPosition
        {
            get => _cellPosition ?? null;
            set
            {
                _cellPosition = value;
                IsAboveTerrain = _cellPosition != null ? true : false;
            }
        }

        public static bool IsAboveTerrain { get; private set; } = false;
    }
}