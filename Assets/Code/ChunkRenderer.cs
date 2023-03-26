using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(CursorPositionFinder))]
public class ChunkRenderer : MonoBehaviour
{
    public Vector2Int Position { get; private set; }

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Mesh _mesh;
    private int _meshInstanceId;
    private bool _isBaking = false;
    private bool _isInitialized = false;

    public void SetPosition(Vector2Int position)
    {
        Position = position;
    }

    private void Initialize()
    {
        _isInitialized = true;
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        InitializeMesh();
    }

    private void InitializeMesh()
    {
        _mesh = new();
        _mesh.MarkDynamic();
        _meshFilter.mesh = _mesh;
        _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        _meshInstanceId = _mesh.GetInstanceID();
    }

    public void GenerateMesh()
    {
        if (!_isInitialized)
            Initialize();

        //int size = 101;

        Vector3[] vertices = new Vector3[10201];
        var thisChunk = Terrain.Chunks[Position].Vertices;
        var rightChunk = Terrain.Chunks[new(Position.x + 1, Position.y)].Vertices;
        var upChunk = Terrain.Chunks[new(Position.x, Position.y + 1)].Vertices;
        var diagonalChunk = Terrain.Chunks[new(Position.x + 1, Position.y + 1)].Vertices;
        for (int z = 0; z < 101; z++)
        {
            for (int x = 0; x < 101; x++)
            {
                if (z < 100 && x < 100)
                    vertices[z * 101 + x] = thisChunk[z * 100 + x];
                else if (x == 100 && z < 100)
                    vertices[z * 101 + x] = rightChunk[z * 100];
                else if (z == 100 && x < 100)
                    vertices[z * 101 + x] = upChunk[x];
                else
                    vertices[10200] = diagonalChunk[0];
            }
        }
        int[] triangles = new int[60000];

        int index = 0;
        for (int z = 0; z < 100; z++)
            for (int x = 0; x < 100; x++)
            {
                int i = (z * 101) + x;

                triangles[index] = i;
                triangles[index + 1] = i + 101;
                triangles[index + 2] = i + 102;
                triangles[index + 3] = i;
                triangles[index + 4] = i + 102;
                triangles[index + 5] = i + 1;
                index += 6;
            }
        _mesh.Clear();
        _mesh.SetVertices(vertices);
        SetColors();
        _mesh.SetTriangles(triangles, 0);
        _mesh.SetUVs(0, vertices
            .Select(x => new Vector2(x.x, x.z))
            .ToArray());
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
        StartCoroutine(AssignMeshToColliderCoroutine());
    }

    public void SetColors()
    {
        Color[] colors = new Color[10201];
        var thisChunk = Terrain.Chunks[Position].Colors;
        var rightChunk = Terrain.Chunks[new(Position.x + 1, Position.y)].Colors;
        var upChunk = Terrain.Chunks[new(Position.x, Position.y + 1)].Colors;
        var diagonalChunk = Terrain.Chunks[new(Position.x + 1, Position.y + 1)].Colors;
        for (int z = 0; z < 101; z++)
        {
            for (int x = 0; x < 101; x++)
            {
                if (z < 100 && x < 100)
                    colors[z * 101 + x] = thisChunk[z * 100 + x];
                else if (x == 100 && z < 100)
                    colors[z * 101 + x] = rightChunk[z * 100];
                else if (z == 100 && x < 100)
                    colors[z * 101 + x] = upChunk[x];
                else
                    colors[10200] = diagonalChunk[0];
            }
        }
        _mesh.SetColors(colors);
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
        _meshCollider.sharedMesh = _mesh;
        _isBaking = false;
    }

    private Task BakePhysicsMesh()
    {
        Physics.BakeMesh(_meshInstanceId, false);
        return Task.CompletedTask;
    }
}