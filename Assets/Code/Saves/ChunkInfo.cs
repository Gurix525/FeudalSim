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
        public CellInfo[] Cells;
        public int[] Heights;

        public ChunkInfo(Chunk chunk)
        {
            CellInfo[] cellInfos = new CellInfo[10000];
            int[] heights = new int[10000];
            Position = chunk.Position;
            Cell[] cells = chunk.Cells.Values.ToArray();
            for (int i = 0; i < 10000; i++)
            {
                Cell cell = cells[i];
                cellInfos[i] = new(cell);
                heights[i] = cell.Height;
            }

            Cells = cellInfos;
            Heights = heights;
        }
    }
}