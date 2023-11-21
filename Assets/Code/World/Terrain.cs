using System;
using System.Collections.Generic;
using System.Linq;
using Controls;
using UnityEngine;

namespace World
{
    public static class Terrain
    {
        #region Properties

        public static Dictionary<Vector2Int, Chunk> Chunks { get; } = new();

        #endregion Properties

        #region Public

        public static void Clear()
        {
            Chunks.Clear();
        }

        public static void ModifyHeight(
            Vector2Int cellPosition,
            int deltaHeight,
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
                    neighbour.Color = Cell.PathVerticeColor;
            }
            if (hasToReload)
            {
                foreach (var chunk in GetChunksToReload(cellPosition))
                    TerrainRenderer.ReloadChunk(chunk);
                GrassInstancer.MarkToReload();
            }
        }

        public static void ChangeColor(
            Vector2Int cellPosition,
            Color color,
            bool hasToSetNeighbours = true,
            bool hasToReload = true)
        {
            if (hasToSetNeighbours)
            {
                Cell[] neighbours = Get4Neighbours(cellPosition);
                foreach (var neighbour in neighbours)
                    neighbour.Color = color;
            }
            else
                GetCell(cellPosition).Color = color;
            if (hasToReload)
            {
                foreach (var chunk in GetChunksToReload(cellPosition))
                    TerrainRenderer.ReloadChunk(chunk);
                GrassInstancer.MarkToReload();
            }
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
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
            return Chunks[chunkPosition][verticePosition];
        }

        public static float GetHeight(Vector2 inputPosition)
        {
            Cell cell = GetCell(inputPosition);
            if (cell == null)
                return 0;
            else
                return cell.Height;
        }

        public static float GetHeight(float x, float z)
        {
            return GetHeight(new(x, z));
        }

        public static float GetSteepness(Vector2 inputPosition)
        {
            return GetCell(inputPosition).Steepness;
        }

        public static Color GetColor(Vector2 inputPosition)
        {
            return GetCell(inputPosition).Color;
        }

        public static void SetBuildingMark(
            Vector3Int startPosition,
            BuildingMode buildingMode,
            float rotation,
            bool state)
        {
            BuildingMarkType wallMarkType = rotation == 0
                    ? BuildingMarkType.HorizontalWall
                    : BuildingMarkType.VerticalWall;
            if (buildingMode == BuildingMode.Floor)
            {
                Cell cell = GetCell(new Vector2Int(startPosition.x, startPosition.z));
                cell.SetBuildingMark(BuildingMarkType.Floor, startPosition.y, state);
            }
            if (buildingMode == BuildingMode.BigFloor)
            {
                Cell[] cells = Get4Neighbours(new Vector2Int(startPosition.x, startPosition.z));
                foreach (var cell in cells)
                {
                    cell.SetBuildingMark(BuildingMarkType.Floor, startPosition.y, state);
                }
            }
            if (buildingMode == BuildingMode.ShortWall)
            {
                Cell cell = GetCell(new Vector2Int(startPosition.x, startPosition.z));
                cell.SetBuildingMark(wallMarkType, startPosition.y, state);
            }
            if (buildingMode == BuildingMode.Wall)
            {
                var cell = GetCell(new Vector2Int(startPosition.x, startPosition.z));
                for (int i = 0; i < 2; i++)
                {
                    cell.SetBuildingMark(wallMarkType, startPosition.y + i, state);
                }
            }
            if (buildingMode == BuildingMode.BigWall)
            {
                var firstCell = GetCell(new Vector2Int(startPosition.x, startPosition.z));
                Vector2Int shift = rotation == 0F ? new(1, 0) : new(0, 1);
                var secondCell = GetCell(new Vector2Int(startPosition.x, startPosition.z) + shift);
                for (int i = 0; i < 2; i++)
                {
                    firstCell.SetBuildingMark(wallMarkType, startPosition.y + i, state);
                    secondCell.SetBuildingMark(wallMarkType, startPosition.y + i, state);
                }
            }
            GrassInstancer.MarkToReload();
        }

        public static bool IsBuildingPossible(
            Vector3Int startPosition,
            BuildingMode buildingMode,
            float rotation)
        {
            BuildingMarkType wallMarkType = rotation == 0
                    ? BuildingMarkType.HorizontalWall
                    : BuildingMarkType.VerticalWall;
            if (buildingMode == BuildingMode.Floor)
                return GetCell(new Vector2Int(startPosition.x, startPosition.z))
                    .IsBuildingPossible(BuildingMarkType.Floor, startPosition.y);
            if (buildingMode == BuildingMode.BigFloor)
            {
                int blockedCellsCount = 0;
                Cell[] cells = Get4Neighbours(new Vector2Int(startPosition.x, startPosition.z));
                foreach (var cell in cells)
                    if (!cell.IsBuildingPossible(BuildingMarkType.Floor, startPosition.y))
                        blockedCellsCount++;
                return blockedCellsCount == 0;
            }
            if (buildingMode == BuildingMode.ShortWall)
            {
                return GetCell(new Vector2Int(startPosition.x, startPosition.z))
                    .IsBuildingPossible(wallMarkType, startPosition.y);
            }
            if (buildingMode == BuildingMode.Wall)
            {
                int blockedCellsCount = 0;
                var cell = GetCell(new Vector2Int(startPosition.x, startPosition.z));
                for (int i = 0; i < 2; i++)
                {
                    if (!cell.IsBuildingPossible(wallMarkType, startPosition.y + i))
                        blockedCellsCount++;
                }
                return blockedCellsCount == 0;
            }
            if (buildingMode == BuildingMode.BigWall)
            {
                int blockedCellsCount = 0;
                var firstCell = GetCell(new Vector2Int(startPosition.x, startPosition.z));
                Vector2Int shift = rotation == 0F ? new(1, 0) : new(0, 1);
                var secondCell = GetCell(new Vector2Int(startPosition.x, startPosition.z) + shift);
                for (int i = 0; i < 2; i++)
                {
                    if (!firstCell.IsBuildingPossible(wallMarkType, startPosition.y + i)
                        || !secondCell.IsBuildingPossible(wallMarkType, startPosition.y + i))
                        blockedCellsCount++;
                }
                return blockedCellsCount == 0;
            }
            return false;
        }

        public static Vector2Int GetChunkCoordinates(Vector2 inputPosition)
        {
            Vector2Int position = new(
                (int)Mathf.Floor(inputPosition.x),
                (int)Mathf.Floor(inputPosition.y));
            return GetChunkCoordinates(position);
        }

        public static Vector2Int GetChunkCoordinates(Vector2Int inputPosition)
        {
            Vector2Int chunkPosition = new(
                (int)Mathf.Floor(inputPosition.x / 50F),
                (int)Mathf.Floor(inputPosition.y / 50F));
            if (!Chunks.ContainsKey(chunkPosition))
                throw new ArgumentOutOfRangeException(
                    "Given position data doesn't exist.");
            return chunkPosition;
        }

        #endregion Public

        #region Private

        private static Vector2Int[] GetChunksToReload(Vector2Int cellPosition)
        {
            List<Vector2Int> chunkPositions = new();
            Vector2Int centralChunk = GetChunkCoordinates(cellPosition);
            chunkPositions.Add(centralChunk);
            Vector2Int vertice = GetVerticeCoordinates(cellPosition);
            if (vertice.x <= 1)
                chunkPositions.Add(new(centralChunk.x - 1, centralChunk.y));
            else if (vertice.x >= 48)
                chunkPositions.Add(new(centralChunk.x + 1, centralChunk.y));
            if (vertice.y <= 1)
                chunkPositions.Add(new(centralChunk.x, centralChunk.y - 1));
            else if (vertice.y >= 48)
                chunkPositions.Add(new(centralChunk.x, centralChunk.y + 1));
            if (chunkPositions.Count == 3)
                chunkPositions.Add(new(chunkPositions[1].x, chunkPositions[2].y));
            return chunkPositions.ToArray();
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

        private static Vector2Int GetVerticeCoordinates(Vector2 inputPosition)
        {
            return new(
                (int)(inputPosition.x % 50 < 0
                    ? 50 - Mathf.Abs(inputPosition.x % 50)
                    : inputPosition.x % 50),
                (int)(inputPosition.y % 50 < 0
                    ? 50 - Mathf.Abs(inputPosition.y % 50)
                    : inputPosition.y % 50));
        }

        #endregion Private
    }
}