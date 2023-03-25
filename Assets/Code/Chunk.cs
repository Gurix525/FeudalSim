using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chunk
{
    #region Properties

    public Vector2Int Position { get; }
    public int X => Position.x;
    public int Z => Position.y;

    public Vector3[] Vertices => _cells.Values
        .Select(cell => new Vector3(
            cell.Position.x,
            cell.Height,
            cell.Position.y)).ToArray();

    private Dictionary<Vector2Int, Cell> _cells = new();

    #endregion Properties

    #region Public

    public Cell this[int x, int z]
    {
        get => _cells[new(x, z)];
    }

    public Cell this[Vector2Int inputPosition]
    {
        get => _cells[inputPosition];
    }

    public Chunk(Vector2Int position)
    {
        Position = position;
        GenerateCells();
        CalculateSteepness();
    }

    #endregion Public

    #region Private

    private void GenerateCells()
    {
        for (int z = 0; z < 100; z++)
            for (int x = 0; x < 100; x++)
                _cells.Add(
                    new(x, z),
                    new Cell(
                        new Vector2Int(
                            Position.x * 100 + x,
                            Position.y * 100 + z),
                        NoiseSampler.GetHeight(
                            Position.x * 100 + x,
                            Position.y * 100 + z)
                        ));
    }

    public void RecalculateBorderSteepness()
    {
        for (int x = Position.x * 100; x < Position.x * 100 + 100; x++)
        {
            float min = Mathf.Min(
                Terrain.GetHeight(new(x, Position.y * 100 + 99)),
                Terrain.GetHeight(new(x + 1, Position.y * 100 + 99)),
                Terrain.GetHeight(new(x, Position.y * 100 + 100)),
                Terrain.GetHeight(new(x + 1, Position.y * 100 + 100)));
            float max = Mathf.Max(
                Terrain.GetHeight(new(x, Position.y * 100 + 99)),
                Terrain.GetHeight(new(x + 1, Position.y * 100 + 99)),
                Terrain.GetHeight(new(x, Position.y * 100 + 100)),
                Terrain.GetHeight(new(x + 1, Position.y * 100 + 100)));
            Terrain.GetCell(new Vector2(x, Position.y * 100 + 99))
                .SetSteepness(Mathf.Round((max - min) * 2F) / 2F);
        }
        for (int z = Position.y * 100; z < Position.y * 100 + 100; z++)
        {
            float min = Mathf.Min(
                Terrain.GetHeight(new(Position.x * 100 + 99, z)),
                Terrain.GetHeight(new(Position.x * 100 + 99, z + 1)),
                Terrain.GetHeight(new(Position.x * 100 + 100, z)),
                Terrain.GetHeight(new(Position.x * 100 + 100, z + 1)));
            float max = Mathf.Max(
                Terrain.GetHeight(new(Position.x * 100 + 99, z)),
                Terrain.GetHeight(new(Position.x * 100 + 99, z + 1)),
                Terrain.GetHeight(new(Position.x * 100 + 100, z)),
                Terrain.GetHeight(new(Position.x * 100 + 100, z + 1)));
            Terrain.GetCell(new Vector2(Position.x * 100 + 99, z))
                .SetSteepness(Mathf.Round((max - min) * 2F) / 2F);
        }
    }

    private void CalculateSteepness()
    {
        for (int z = 0; z < 100; z++)
            for (int x = 0; x < 100; x++)
            {
                float min = 0F;
                float max = 0F;
                if (x < 99 && z < 99)
                {
                    min = Mathf.Min(
                        _cells[new(x, z)].Height,
                        _cells[new(x + 1, z)].Height,
                        _cells[new(x, z + 1)].Height,
                        _cells[new(x + 1, z + 1)].Height);
                    max = Mathf.Max(
                        _cells[new(x, z)].Height,
                        _cells[new(x + 1, z)].Height,
                        _cells[new(x, z + 1)].Height,
                        _cells[new(x + 1, z + 1)].Height);
                }
                else if (x < 99 && z == 99)
                {
                    min = Mathf.Min(
                        _cells[new(x, z)].Height,
                        _cells[new(x + 1, z)].Height);
                    max = Mathf.Max(
                        _cells[new(x, z)].Height,
                        _cells[new(x + 1, z)].Height);
                }
                else if (x == 99 && z < 99)
                {
                    min = Mathf.Min(
                        _cells[new(x, z)].Height,
                        _cells[new(x, z + 1)].Height);
                    max = Mathf.Max(
                        _cells[new(x, z)].Height,
                        _cells[new(x, z + 1)].Height);
                }
                else
                {
                    min = 0;
                    max = 0;
                }
                _cells[new(x, z)].SetSteepness(Mathf.Round((max - min) * 2F) / 2F);
            }
    }

    #endregion Private
}