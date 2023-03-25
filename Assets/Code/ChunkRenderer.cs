using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{
    public Vector2Int Position { get; private set; }

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Mesh _mesh;
    private int[] _triangles = new int[58806];
    private Vector3[] _vertices = new Vector3[10000];
    private Color[] _colors = new Color[10000];
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
        int size = 99;

        _vertices = Terrain.Chunks[Position].Vertices;

        int index = 0;
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
        _mesh.Clear();
        _mesh.SetVertices(_vertices);
        _mesh.SetColors(_colors);
        _mesh.SetTriangles(_triangles, 0);
        _mesh.SetUVs(0, _vertices
            .Select(x => new Vector2(x.x, x.z))
            .ToArray());
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
        StartCoroutine(AssignMeshToColliderCoroutine());
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