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

    public float GetSteepness(Vector2Int position)
    {
        return _steepnesses[position.x, position.y];
    }

    public void RecalculateBorderSteepness()
    {
        float[] nextXses = new float[100];
        float[] nextZses = new float[100];
        float nextXZ = Terrain.Chunks[new(X + 1, Z + 1)][0, 0];
        for (int i = 0; i < 100; i++)
        {
            nextXses[i] = Terrain.Chunks[new(X, Z + 1)][i, 0];
            nextZses[i] = Terrain.Chunks[new(X + 1, Z)][0, i];
        }
        for (int i = 0; i < 100; i++)
        {
            if (i < 99)
            {
                float minX = Mathf.Min(_heights[i, 99], _heights[i + 1, 99], nextXses[i], nextXses[i + 1]);
                float maxX = Mathf.Max(_heights[i, 99], _heights[i + 1, 99], nextXses[i], nextXses[i + 1]);
                float minZ = Mathf.Min(_heights[99, i], _heights[99, i + 1], nextZses[i], nextZses[i + 1]);
                float maxZ = Mathf.Max(_heights[99, i], _heights[99, i + 1], nextZses[i], nextZses[i + 1]);
                _steepnesses[i, 99] = Mathf.Round((maxX - minX) * 2F) / 2F;
                _steepnesses[99, i] = Mathf.Round((maxZ - minZ) * 2F) / 2F;
            }
            else
            {
                float min = Mathf.Min(_heights[99, 99], nextXses[99], nextZses[99], nextXZ);
                float max = Mathf.Max(_heights[99, 99], nextXses[99], nextZses[99], nextXZ);
                _steepnesses[99, 99] = Mathf.Round((max - min) * 2F) / 2F;
            }
        }
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