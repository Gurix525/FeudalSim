using UnityEngine;

public class Chunk
{
    public Vector2Int Position { get; }
    public int X => Position.x;
    public int Z => Position.y;

    private float[,] _heights { get; }
    private float[,] _steepnesses { get; }

    public float this[int x, int z]
    {
        get => _heights[x, z];
    }

    public Chunk(Vector2Int position)
    {
        Position = position;
        _heights = GenerateHeights();
        _steepnesses = CalculateSteepness();
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

    private float[,] CalculateSteepness()
    {
        float[,] steepnesses = new float[100, 100];
        for (int z = 0; z < 100; z++)
            for (int x = 0; x < 100; x++)
            {
                float min = 0F;
                float max = 0F;
                if (x < 99 && z < 99)
                {
                    min = Mathf.Min(_heights[x, z], _heights[x + 1, z], _heights[x, z + 1], _heights[x + 1, z + 1]);
                    max = Mathf.Max(_heights[x, z], _heights[x + 1, z], _heights[x, z + 1], _heights[x + 1, z + 1]);
                }
                else if (x < 99 && z == 99)
                {
                    min = Mathf.Min(_heights[x, z], _heights[x + 1, z]);
                    max = Mathf.Max(_heights[x, z], _heights[x + 1, z]);
                }
                else if (x == 99 && z < 99)
                {
                    min = Mathf.Min(_heights[x, z], _heights[x, z + 1]);
                    max = Mathf.Max(_heights[x, z], _heights[x, z + 1]);
                }
                else
                {
                    min = 0;
                    max = 0;
                }
                steepnesses[x, z] = Mathf.Round((max - min) * 2F) / 2F;
            }
        return steepnesses;
    }
}