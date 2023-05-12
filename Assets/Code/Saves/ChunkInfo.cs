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
        public string[] FloorHeights;
        public string[] HorizontalWallHeights;
        public string[] VerticalWallHeights;

        public ChunkInfo(Chunk chunk)
        {
            Position = chunk.Position;
            Heights = new int[10000];
            Colors = new Color[10000];
            FloorHeights = new string[10000];
            HorizontalWallHeights = new string[10000];
            VerticalWallHeights = new string[10000];

            Cell[] cells = chunk.Cells.Values.ToArray();
            for (int i = 0; i < 10000; i++)
            {
                Cell cell = cells[i];
                Heights[i] = cell.Height;
                Colors[i] = cell.Color;
                FloorHeights[i] = String.Join(
                    ',',
                    cell.FloorHeights
                    .Select(x => x.ToString())
                    .ToArray());
                HorizontalWallHeights[i] = String.Join(
                    ',',
                    cell.HorizontalWallHeights
                    .Select(x => x.ToString())
                    .ToArray());
                VerticalWallHeights[i] = String.Join(
                    ',',
                    cell.VerticalWallHeights
                    .Select(x => x.ToString())
                    .ToArray());
            }
        }
    }
}