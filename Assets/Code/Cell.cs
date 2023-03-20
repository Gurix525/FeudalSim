using System;
using UnityEngine;

public class Cell
{
    private Vector2Int _position;
    private float _height;
    private float _steepness;

    public Vector2Int Position => _position;
    public float Height => _height;
    public float Steepness => _steepness;

    public Cell(Vector2Int position, float height, float steepness = 0F)
    {
        _position = position;
        _height = height;
        _steepness = steepness;
    }

    public void RecalculateSteepness()
    {
        float min = Mathf.Min(
            Height,
            Terrain.GetHeight(new Vector2Int(Position.x + 1, Position.y)),
            Terrain.GetHeight(new Vector2Int(Position.x, Position.y + 1)),
            Terrain.GetHeight(new Vector2Int(Position.x + 1, Position.y + 1)));
        float max = Mathf.Max(
            Height,
            Terrain.GetHeight(new Vector2Int(Position.x + 1, Position.y)),
            Terrain.GetHeight(new Vector2Int(Position.x, Position.y + 1)),
            Terrain.GetHeight(new Vector2Int(Position.x + 1, Position.y + 1)));
        _steepness = max - min;
    }

    public void ModifyHeight(float deltaHeight)
    {
        _height += deltaHeight;
    }

    public void SetHeight(float height)
    {
        _height = height;
    }

    public void SetSteepness(float steepness)
    {
        _steepness = steepness;
    }

    public override string ToString()
    {
        return $"{Height}";
    }
}