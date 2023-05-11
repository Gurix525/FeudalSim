using System;
using UnityEngine;
using World;

namespace Saves
{
    [Serializable]
    public class CellInfo
    {
        public Vector2Int Position;
        public int Height;
        public Color Color;
        public int[] FloorHeights;
        public int[] HorizontalWallHeights;
        public int[] VerticalWallHeights;

        public CellInfo(Cell cell)
        {
            Position = cell.Position;
            Height = cell.Height;
            Color = cell.Color;
            FloorHeights = cell.FloorHeights.ToArray();
            HorizontalWallHeights = cell.HorizontalWallHeights.ToArray();
            VerticalWallHeights = cell.VerticalWallHeights.ToArray();
        }
    }
}