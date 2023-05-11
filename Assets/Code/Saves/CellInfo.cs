using System;
using UnityEngine;
using World;

namespace Saves
{
    [Serializable]
    public class CellInfo
    {
        public Color Color;
        public int[] FloorHeights;
        public int[] HorizontalWallHeights;
        public int[] VerticalWallHeights;

        public CellInfo(Cell cell)
        {
            Color = cell.Color;
            FloorHeights = cell.FloorHeights.ToArray();
            HorizontalWallHeights = cell.HorizontalWallHeights.ToArray();
            VerticalWallHeights = cell.VerticalWallHeights.ToArray();
        }
    }
}