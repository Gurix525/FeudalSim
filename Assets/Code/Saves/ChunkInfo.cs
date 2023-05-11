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

        public ChunkInfo(Chunk chunk)
        {
            CellInfo[] cells = new CellInfo[10000];
            Position = chunk.Position;
            for (int i = 0; i < 10000; i++)
            {
                cells[i] = new(chunk.Cells.Values.ToArray()[i]);
            }
            Cells = cells;
        }
    }
}