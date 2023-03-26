using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace World
{
    public static class Terrain
    {
        public static Dictionary<Vector2Int, Chunk> Chunks { get; } = new();

        public static void ModifyHeight(
            Vector2Int cellPosition,
            float deltaHeight,
            bool hasToSetNeighbours = true,
            bool hasToReload = true,
            bool hasToChangeColor = true)
        {
            Cell[] neighbours = Get4Neighbours(cellPosition);
            if (hasToSetNeighbours)
            {
                float limit = deltaHeight < 0
                    ? neighbours.Select(x => x.Height).Min()
                    : neighbours.Select(x => x.Height).Max();
                if (neighbours[0].Steepness > 0F)
                    foreach (var neighbour in neighbours)
                    {
                        if ((deltaHeight < 0 && neighbour.Height > limit)
                            || (deltaHeight > 0 && neighbour.Height < limit))
                            neighbour.ModifyHeight(deltaHeight);
                    }
                else
                    foreach (var neighbour in neighbours)
                        neighbour.ModifyHeight(deltaHeight);
                foreach (var neighbour in Get9Neighbours(cellPosition))
                    neighbour.RecalculateSteepness();
            }
            else
            {
                neighbours[0].ModifyHeight(deltaHeight);
                foreach (var neighbour in Get9Neighbours(cellPosition))
                    neighbour.RecalculateSteepness();
            }
            if (hasToChangeColor)
            {
                foreach (var neighbour in neighbours)
                    neighbour.SetColor(Color.red);
            }
            if (hasToReload)
            {
                foreach (var chunk in GetChunksToReload(cellPosition))
                    TerrainRenderer.ReloadChunk(chunk);
                GrassInstancer.MarkToReload();
            }
        }

        internal static void ChangeColor(
            Vector2Int cellPosition,
            Color color,
            bool hasToSetNeighbours = true,
            bool hasToReload = true)
        {
            if (hasToSetNeighbours)
            {
                Cell[] neighbours = Get4Neighbours(cellPosition);
                foreach (var neighbour in neighbours)
                    neighbour.SetColor(color);
            }
            else
                GetCell(cellPosition).SetColor(color);
            if (hasToReload)
            {
                foreach (var chunk in GetChunksToReload(cellPosition))
                    TerrainRenderer.ReloadChunk(chunk);
                GrassInstancer.MarkToReload();
            }
        }

        private static Vector2Int[] GetChunksToReload(Vector2Int cellPosition)
        {
            List<Vector2Int> chunks = new();
            Vector2Int centralChunk = GetChunkCoordinates(cellPosition);
            chunks.Add(centralChunk);
            Vector2Int vertice = GetVerticeCoordinates(cellPosition);
            if (vertice.x <= 1)
                chunks.Add(new(centralChunk.x - 1, centralChunk.y));
            else if (vertice.x >= 98)
                chunks.Add(new(centralChunk.x + 1, centralChunk.y));
            if (vertice.y <= 1)
                chunks.Add(new(centralChunk.x, centralChunk.y - 1));
            else if (vertice.y >= 98)
                chunks.Add(new(centralChunk.x, centralChunk.y + 1));
            if (chunks.Count == 3)
                chunks.Add(new(chunks[1].x, chunks[2].y));
            return chunks.ToArray();
        }

        private static Cell[] Get4Neighbours(Vector2Int cellPosition)
        {
            int x = cellPosition.x;
            int z = cellPosition.y;
            return new Cell[4]
            {
            GetCell(cellPosition),
            GetCell(new Vector2Int(x + 1, z)),
            GetCell(new Vector2Int(x, z + 1)),
            GetCell(new Vector2Int(x + 1, z + 1))
            };
        }

        private static Cell[] Get9Neighbours(Vector2Int cellPosition)
        {
            int originalX = cellPosition.x;
            int originalZ = cellPosition.y;
            Cell[] cells = new Cell[9];
            int index = 0;
            for (int z = originalZ - 1; z < originalZ + 2; z++)
                for (int x = originalX - 1; x < originalX + 2; x++)
                {
                    cells[index++] = GetCell(new Vector2Int(x, z));
                }
            return cells;
        }

        public static Cell GetCell(Vector2Int cellPosition)
        {
            return GetCell(new Vector2(cellPosition.x, cellPosition.y));
        }

        public static Cell GetCell(Vector3 inputPosition)
        {
            return GetCell(new Vector2(inputPosition.x, inputPosition.z));
        }

        public static Cell GetCell(Vector2 inputPosition)
        {
            Vector2Int chunkPosition = Vector2Int.zero;
            Vector2Int verticePosition = GetVerticeCoordinates(inputPosition);
            try
            {
                chunkPosition = GetChunkCoordinates(inputPosition);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogError(e.Message);
                return null;
            }
            return Chunks[chunkPosition][verticePosition];
        }

        public static float GetHeight(Vector2 inputPosition)
        {
            return GetCell(inputPosition).Height;
        }

        public static float GetSteepness(Vector2 inputPosition)
        {
            return GetCell(inputPosition).Steepness;
        }

        public static Color GetColor(Vector2 inputPosition)
        {
            return GetCell(inputPosition).Color;
        }

        private static Vector2Int GetChunkCoordinates(Vector2 inputPosition)
        {
            Vector2Int position = new(
                (int)Mathf.Floor(inputPosition.x),
                (int)Mathf.Floor(inputPosition.y));

            Vector2Int chunkPosition = new(
                (int)Mathf.Floor(position.x / 100F),
                (int)Mathf.Floor(position.y / 100F));
            if (!Chunks.ContainsKey(chunkPosition))
                throw new ArgumentOutOfRangeException(
                    "Given position data doesn't exist.");
            return chunkPosition;
        }

        private static Vector2Int GetVerticeCoordinates(Vector2 inputPosition)
        {
            return new(
                (int)(inputPosition.x % 100 < 0
                    ? 100 - Mathf.Abs(inputPosition.x % 100)
                    : inputPosition.x % 100),
                (int)(inputPosition.y % 100 < 0
                    ? 100 - Mathf.Abs(inputPosition.y % 100)
                    : inputPosition.y % 100));
        }
    }
}