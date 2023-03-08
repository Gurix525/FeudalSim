using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class TerrainGenerator : MonoBehaviour
{
    public static TerrainGenerator Instance { get; private set; }

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Dictionary<Vector2Int, Chunk> _chunks = new();
    private Chunk _activeChunk;

    public Chunk ActiveChunk
    {
        get => _activeChunk;
        set
        {
            _activeChunk = value;
            UpdateTerrain();
        }
    }

    public static void ChangeActiveChunk(Vector2Int chunkPos)
    {
        if (Instance.ActiveChunk.Position != chunkPos)
            Instance.ActiveChunk = Instance._chunks[chunkPos];
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        Instance = this;
        UpdateTerrain();
    }

    private void UpdateTerrain()
    {
        Profiler.BeginSample("UpdateTerrain");
        GenerateChunks();
        Mesh mesh = GenerateMesh();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
        Profiler.EndSample();
    }

    private void GenerateChunks()
    {
        Profiler.BeginSample("GenerateChunks");
        if (ActiveChunk == null)
        {
            _chunks.Add(new(0, 0), new(new(0, 0)));
            ActiveChunk = _chunks.First().Value;
        }
        for (int x = ActiveChunk.X - 1; x <= ActiveChunk.X + 1; x++)
            for (int z = ActiveChunk.Z - 1; z <= ActiveChunk.Z + 1; z++)
            {
                if (x == ActiveChunk.X && z == ActiveChunk.Z)
                    continue;
                if (!_chunks.ContainsKey(new(x, z)))
                    _chunks.Add(new(x, z), new(new(x, z)));
            }
        Profiler.EndSample();
    }

    private Mesh GenerateMesh()
    {
        Profiler.BeginSample("GenerateMesh");
        List<Vector3> vertices = new();
        List<int> triangles = new();
        int size = 299;

        Profiler.BeginSample("VertexAssigning");
        for (int z = 0; z < size + 1; z++)
            for (int x = 0; x < size + 1; x++)
            {
                int chunkZ = 0;
                int chunkX = 0;

                if (z < 100)
                    chunkZ = _activeChunk.Z - 1;
                else if (z < 200)
                    chunkZ = _activeChunk.Z;
                else
                    chunkZ = _activeChunk.Z + 1;

                if (x < 100)
                    chunkX = _activeChunk.X - 1;
                else if (x < 200)
                    chunkX = _activeChunk.X;
                else
                    chunkX = _activeChunk.X + 1;

                vertices.Add(new(chunkX * 100 + x % 100, _chunks[new(chunkX, chunkZ)][x % 100, z % 100], chunkZ * 100 + z % 100));
            }
        Profiler.EndSample();
        Profiler.BeginSample("CreatingTriangles");
        for (int z = 0; z < size - 1; z++)
            for (int x = 0; x < size - 1; x++)
            {
                int i = (z * size) + z + x;

                triangles.Add(i);
                triangles.Add(i + size + 1);
                triangles.Add(i + size + 2);
                triangles.Add(i);
                triangles.Add(i + size + 2);
                triangles.Add(i + 1);
            }
        Profiler.EndSample();
        Profiler.BeginSample("CreatingMesh");
        Mesh mesh = new();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        Profiler.EndSample();
        Profiler.BeginSample("Creating UV");
        mesh.uv = vertices
            .Select(x => new Vector2(x.x, x.z))
            .ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        Profiler.EndSample();
        Profiler.EndSample();
        return mesh;
    }
}

public class Chunk
{
    public Vector2Int Position { get; }
    public int X => Position.x;
    public int Z => Position.y;

    private static readonly float _detailScale = 0.007F;
    private static readonly float _frequencyFactor = 2F;
    private static readonly float _amplitudeFactor = 2F;
    private static readonly float _maxHeight = 10F;
    private static readonly int _octaves = 3;

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
                heights[x, z] = GetPerlinHeight(Position.x * 100 + x, Position.y * 100 + z, _octaves);
            }
        return heights;
    }

    private static float GetPerlinHeight(int x, int z, int octaves)
    {
        float result = 0F;
        float resultDivider = 0F;
        for (int i = 0; i < octaves; i++)
        {
            result += Mathf.PerlinNoise(
                x * _detailScale * Mathf.Pow(_frequencyFactor, i),
                z * _detailScale * Mathf.Pow(_frequencyFactor, i))
                / Mathf.Pow(_amplitudeFactor, i);
            resultDivider += 1F / Mathf.Pow(_amplitudeFactor, i);
        }
        result /= resultDivider;
        float remapped = result.Remap(0.4F, 0.6F, 0F, _maxHeight);
        return Mathf.Round(remapped * 2F) / 2F;
    }
}