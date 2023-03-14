using UnityEngine;

public class Chunk
{
    public Vector2Int Position { get; }
    public int X => Position.x;
    public int Z => Position.y;

    private float[,] _heights { get; }

    public float this[int x, int z]
    {
        get => _heights[x, z];
    }

    public Chunk(Vector2Int position)
    {
        Position = position;
        _heights = GenerateHeights();
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[100, 100];
        for (int z = 0; z < 100; z++)
            for (int x = 0; x < 100; x++)
            {
                heights[x, z] = NoiseSampler.GetHeight(Position.x * 100 + x, Position.y * 100 + z);
            }
        return heights;
    }
}