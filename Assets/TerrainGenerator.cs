using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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
    private Mesh _mesh;
    private int[] _triangles = new int[532824];
    private Vector3[] _vertices = new Vector3[90000];
    private int _meshInstanceId;
    private bool _isBaking = false;

    public Chunk ActiveChunk
    {
        get => _activeChunk;
        set
        {
            _activeChunk = value;
            UpdateTerrain();
        }
    }

    public Dictionary<Vector2Int, Chunk> Chunks
        => _chunks;

    private void Awake()
    {
        Instance = this;
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        InitializeMesh();
        UpdateTerrain();
    }

    private void InitializeMesh()
    {
        _mesh = new();
        _mesh.MarkDynamic();
        _meshFilter.mesh = _mesh;
        _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        _meshInstanceId = _mesh.GetInstanceID();
    }

    private IEnumerable<Vector2> GetUvs()
    {
        for (int z = -100; z < 200; z++)
            for (int x = -100; x < 200; x++)
                yield return new Vector2(x, z);
    }

    private void UpdateTerrain()
    {
        Profiler.BeginSample("UpdateTerrain");
        GenerateChunks();
        GenerateMesh();
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

    private void GenerateMesh()
    {
        Profiler.BeginSample("GenerateMesh");
        int size = 299;

        int index = 0;
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

                _vertices[index] = new(chunkX * 100 + x % 100, _chunks[new(chunkX, chunkZ)][x % 100, z % 100], chunkZ * 100 + z % 100);
                index++;
            }
        Profiler.EndSample();
        Profiler.BeginSample("CreatingTriangles");
        index = 0;
        for (int z = 0; z < size - 1; z++)
            for (int x = 0; x < size - 1; x++)
            {
                int i = (z * size) + z + x;

                _triangles[index] = i;
                _triangles[index + 1] = i + size + 1;
                _triangles[index + 2] = i + size + 2;
                _triangles[index + 3] = i;
                _triangles[index + 4] = i + size + 2;
                _triangles[index + 5] = i + 1;
                index += 6;
            }
        Profiler.EndSample();
        Profiler.BeginSample("CreatingMesh");
        _mesh.Clear();
        _mesh.SetVertices(_vertices);
        _mesh.SetTriangles(_triangles, 0);
        Profiler.EndSample();
        Profiler.BeginSample("Creating UV");
        _mesh.SetUVs(0, _vertices
            .Select(x => new Vector2(x.x, x.z))
            .ToArray());
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
        Profiler.EndSample();
        StartCoroutine(AssignMeshToColliderCoroutine());
        Profiler.EndSample();
    }

    private Task BakePhysicsMesh()
    {
        Debug.Log("Task started");
        try
        {
            Physics.BakeMesh(_meshInstanceId, false);
            Debug.Log("MeshBaked");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        Debug.Log("Task finished");
        return Task.CompletedTask;
    }

    private IEnumerator AssignMeshToColliderCoroutine()
    {
        if (_isBaking)
            yield break;
        _isBaking = true;
        var task = Task.Run(BakePhysicsMesh);
        while (true)
        {
            if (task.IsCompleted)
                break;
            if (Input.GetKeyDown(KeyCode.U))
                break;
            yield return null;
        }
        Debug.Log("Finished coroutine");
        Profiler.BeginSample("Assigning mesh to collider");
        _meshCollider.sharedMesh = _mesh;
        _isBaking = false;
        Profiler.EndSample();
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