using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class TerrainGenerator : MonoBehaviour
{
    public static TerrainGenerator Instance { get; private set; }
    public static UnityEvent<Vector2> TerrainUpdating { get; private set; } = new();
    public Vector3[] Vertices => _vertices;

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Mesh _mesh;
    private int[] _triangles = new int[532824];
    private Vector3[] _vertices = new Vector3[90000];
    private int _meshInstanceId;
    private bool _isBaking = false;

    public Chunk ActiveChunk { get; private set; }

    public static void SetActiveChunk(Vector2Int position)
    {
        GenerateChunks(position);
        Instance.ActiveChunk = Terrain.Chunks[position];
        UpdateTerrain();
    }

    private void Awake()
    {
        Instance = this;
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        InitializeMesh();
        GenerateChunks(Vector2Int.zero);
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

    private static void UpdateTerrain()
    {
        Profiler.BeginSample("UpdateTerrain");
        Instance.GenerateMesh();
        TerrainUpdating.Invoke(Instance.ActiveChunk.Position);
        RecalculateActiveChunkBorderSteepness();
        Profiler.EndSample();
    }

    private static void RecalculateActiveChunkBorderSteepness()
    {
        Terrain.Chunks[Instance.ActiveChunk.Position].RecalculateBorderSteepness();
    }

    private static void GenerateChunks(Vector2Int activePosition)
    {
        Profiler.BeginSample("GenerateChunks");
        for (int x = activePosition.x - 1; x <= activePosition.x + 1; x++)
            for (int z = activePosition.y - 1; z <= activePosition.y + 1; z++)
            {
                if (!Terrain.Chunks.ContainsKey(new(x, z)))
                    Terrain.Chunks.Add(new(x, z), new(new(x, z)));
            }
        Instance.ActiveChunk ??= Terrain.Chunks[activePosition];
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
                    chunkZ = ActiveChunk.Z - 1;
                else if (z < 200)
                    chunkZ = ActiveChunk.Z;
                else
                    chunkZ = ActiveChunk.Z + 1;

                if (x < 100)
                    chunkX = ActiveChunk.X - 1;
                else if (x < 200)
                    chunkX = ActiveChunk.X;
                else
                    chunkX = ActiveChunk.X + 1;

                _vertices[index] = new(chunkX * 100 + x % 100, Terrain.Chunks[new(chunkX, chunkZ)][x % 100, z % 100], chunkZ * 100 + z % 100);
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
        Physics.BakeMesh(_meshInstanceId, false);
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
            if (UnityEngine.Input.GetKeyDown(KeyCode.U))
                break;
            yield return null;
        }
        Profiler.BeginSample("Assigning mesh to collider");
        _meshCollider.sharedMesh = _mesh;
        _isBaking = false;
        Profiler.EndSample();
    }
}