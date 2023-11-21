using System;
using System.Collections.Generic;
using System.Linq;
using Saves;
using UnityEngine;

namespace World
{
    public class Chunk
    {
        #region Properties

        public Vector2Int Position { get; }

        public bool IsNatureSpawned { get; set; }

        public Dictionary<Vector2Int, Cell> Cells { get; private set; } = new();

        public Vector3[] Vertices => Cells.Values
            .Select(cell => new Vector3(
                cell.Position.x,
                cell.Height,
                cell.Position.y))
            .ToArray();

        public Color[] Colors => Cells.Values
            .Select(cell => cell.Color)
            .ToArray();

        public static Chunk Empty => new();

        #endregion Properties

        #region Constructors

        public Chunk(Vector2Int position)
        {
            Position = position;
            GenerateCells();
            CalculateSteepness();
        }

        public Chunk(ChunkInfo chunkInfo)
        {
            Position = chunkInfo.Position;
            LoadHeights(chunkInfo);
            int index = 0;
            foreach (Cell cell in Cells.Values)
            {
                cell.Color = chunkInfo.Colors[index];
                cell.Steepness = chunkInfo.Steepnesses[index];
                cell.FloorHeights = chunkInfo.FloorHeights[index]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(number => int.Parse(number)).ToList();
                cell.HorizontalWallHeights = chunkInfo.HorizontalWallHeights[index]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(number => int.Parse(number)).ToList();
                cell.VerticalWallHeights = chunkInfo.VerticalWallHeights[index]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(number => int.Parse(number)).ToList();
                index++;
            }
            CalculateSteepness();
        }

        private Chunk()
        {
        }

        #endregion Constructors

        #region Public

        public Cell this[int x, int z]
        {
            get => Cells[new(x, z)];
        }

        public Cell this[Vector2Int inputPosition]
        {
            get => Cells[inputPosition];
        }

        #endregion Public

        #region Private

        private void GenerateCells()
        {
            for (int z = 0; z < 50; z++)
                for (int x = 0; x < 50; x++)
                {
                    var cellPosition = new Vector2Int(
                                Position.x * 50 + x,
                                Position.y * 50 + z);
                    var cellHeight = NoiseSampler.GetHeight(
                                Position.x * 50 + x,
                                Position.y * 50 + z);
                    Color cellColor = GetCellColor(cellPosition, cellHeight);
                    Cells.Add(
                        new Vector2Int(x, z),
                        new Cell(cellPosition, cellHeight, color: cellColor));
                }
        }

        private Color GetCellColor(Vector2Int cellPosition, int cellHeight)
        {
            return cellHeight > 0 ? Cell.GrassVerticeColor : Cell.SandVerticeColor;
        }

        private void LoadHeights(ChunkInfo chunkInfo)
        {
            for (int z = 0; z < 50; z++)
                for (int x = 0; x < 50; x++)
                    Cells.Add(
                        new(x, z),
                        new Cell(
                            new Vector2Int(
                                Position.x * 50 + x,
                                Position.y * 50 + z),
                            chunkInfo.Heights[z * 50 + x]));
        }

        public void RecalculateBorderSteepness()
        {
            for (int x = Position.x * 50; x < Position.x * 50 + 50; x++)
            {
                float min = Mathf.Min(
                    Terrain.GetHeight(new(x, Position.y * 50 + 49)),
                    Terrain.GetHeight(new(x + 1, Position.y * 50 + 49)),
                    Terrain.GetHeight(new(x, Position.y * 50 + 50)),
                    Terrain.GetHeight(new(x + 1, Position.y * 50 + 50)));
                float max = Mathf.Max(
                    Terrain.GetHeight(new(x, Position.y * 50 + 49)),
                    Terrain.GetHeight(new(x + 1, Position.y * 50 + 49)),
                    Terrain.GetHeight(new(x, Position.y * 50 + 50)),
                    Terrain.GetHeight(new(x + 1, Position.y * 50 + 50)));
                Terrain.GetCell(new Vector2(x, Position.y * 50 + 49))
                    .Steepness = Mathf.Round((max - min) * 2F) / 2F;
            }
            for (int z = Position.y * 50; z < Position.y * 50 + 50; z++)
            {
                float min = Mathf.Min(
                    Terrain.GetHeight(new(Position.x * 50 + 49, z)),
                    Terrain.GetHeight(new(Position.x * 50 + 49, z + 1)),
                    Terrain.GetHeight(new(Position.x * 50 + 50, z)),
                    Terrain.GetHeight(new(Position.x * 50 + 50, z + 1)));
                float max = Mathf.Max(
                    Terrain.GetHeight(new(Position.x * 50 + 49, z)),
                    Terrain.GetHeight(new(Position.x * 50 + 49, z + 1)),
                    Terrain.GetHeight(new(Position.x * 50 + 50, z)),
                    Terrain.GetHeight(new(Position.x * 50 + 50, z + 1)));
                Terrain.GetCell(new Vector2(Position.x * 50 + 49, z))
                    .Steepness = Mathf.Round((max - min) * 2F) / 2F;
            }
        }

        private void CalculateSteepness()
        {
            for (int z = 0; z < 50; z++)
                for (int x = 0; x < 50; x++)
                {
                    float min = 0F;
                    float max = 0F;
                    if (x < 49 && z < 49)
                    {
                        min = Mathf.Min(
                            Cells[new(x, z)].Height,
                            Cells[new(x + 1, z)].Height,
                            Cells[new(x, z + 1)].Height,
                            Cells[new(x + 1, z + 1)].Height);
                        max = Mathf.Max(
                            Cells[new(x, z)].Height,
                            Cells[new(x + 1, z)].Height,
                            Cells[new(x, z + 1)].Height,
                            Cells[new(x + 1, z + 1)].Height);
                    }
                    else if (x < 49 && z == 49)
                    {
                        min = Mathf.Min(
                            Cells[new(x, z)].Height,
                            Cells[new(x + 1, z)].Height);
                        max = Mathf.Max(
                            Cells[new(x, z)].Height,
                            Cells[new(x + 1, z)].Height);
                    }
                    else if (x == 49 && z < 49)
                    {
                        min = Mathf.Min(
                            Cells[new(x, z)].Height,
                            Cells[new(x, z + 1)].Height);
                        max = Mathf.Max(
                            Cells[new(x, z)].Height,
                            Cells[new(x, z + 1)].Height);
                    }
                    else
                    {
                        min = 0;
                        max = 0;
                    }
                    Cells[new(x, z)].Steepness = Mathf.Round((max - min) * 2F) / 2F;
                }
        }

        #endregion Private
    }
}