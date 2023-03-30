using Items;
using UnityEngine;

namespace Controls
{
    public static class Cursor
    {
        private static Vector2Int? _cellPosition;

        private static Container _container = new(1);

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