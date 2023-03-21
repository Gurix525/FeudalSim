using System;
using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }
    public float Height { get; private set; }
    public float Steepness { get; private set; }
    public Color Color { get; private set; }

    public Cell(
        Vector2Int position,
        float height,
        float steepness = 0F,
        Color color = new())
    {
        Position = position;
        Height = height;
        Steepness = steepness;
        Color = color;
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
        Steepness = max - min;
    }

    public void ModifyHeight(float deltaHeight)
    {
        Height += deltaHeight;
    }

    public void SetHeight(float height)
    {
        Height = height;
    }

    public void SetSteepness(float steepness)
    {
        Steepness = steepness;
    }

    public void SetColor(Color red)
    {
        Color = red;
    }

    public override string ToString()
    {
        return $"{Position}";
    }
}