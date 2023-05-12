using System;
using System.Linq;
using UnityEngine;
using World;

namespace Saves
{
    [Serializable]
    public class ChunkInfo
    {
        public Vector2Int Position;
        public int[] Heights;
        public Color[] Colors;
        public int[][] FloorHeights;
        public int[][] HorizontalWallHeights;
        public int[][] VerticalWallHeights;

        public ChunkInfo(Chunk chunk)
        {
            Position = chunk.Position;
            Heights = new int[10000];
            Colors = new Color[10000];
            FloorHeights = new int[10000][];
            HorizontalWallHeights = new int[10000][];
            VerticalWallHeights = new int[10000][];

            Cell[] cells = chunk.Cells.Values.ToArray();
            for (int i = 0; i < 10000; i++)
            {
                Cell cell = cells[i];
                Heights[i] = cell.Height;
                Colors[i] = cell.Color;
                FloorHeights[i] = cell.FloorHeights.ToArray();
                HorizontalWallHeights[i] = cell.HorizontalWallHeights.ToArray();
                VerticalWallHeights[i] = cell.VerticalWallHeights.ToArray();
            }
        }
    }
}