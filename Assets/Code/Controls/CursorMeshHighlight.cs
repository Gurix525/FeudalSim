using UnityEngine;
using Cursor = Controls.Cursor;
using Terrain = World.Terrain;

public class CursorMeshHighlight : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _notBlockedMaterial;
    [SerializeField] private Material _blockedMaterial;

    private GameObject _highlight;
    private MeshFilter _filter;
    private MeshRenderer _renderer;

    public static CursorMeshHighlight Instance { get; private set; }
    public static bool IsBlocked { get; set; } = false;

    public static void TrySetMesh(Mesh mesh)
    {
        if (Instance._mesh != mesh)
            Instance._mesh = mesh;
    }

    private void Awake()
    {
        Instance = this;
        _highlight = new GameObject("MeshHighlight");
        _filter = _highlight.AddComponent<MeshFilter>();
        _renderer = _highlight.AddComponent<MeshRenderer>();
        _renderer.material = _notBlockedMaterial;
    }

    private void Update()
    {
        if (_mesh != _filter.mesh)
            _filter.mesh = _mesh;
        if (_renderer.material != _notBlockedMaterial && !IsBlocked)
            _renderer.material = _notBlockedMaterial;
        else if (_renderer.material != _blockedMaterial && IsBlocked)
            _renderer.material = _blockedMaterial;
        if (Cursor.IsAboveTerrain && _mesh != null)
        {
            _renderer.enabled = true;
            _renderer.material.renderQueue = 3001;
            var cellPosition = Cursor.CellPosition.Value;
            _highlight.transform.position = new(
                cellPosition.x + 0.5F, Terrain.GetHeight(cellPosition), cellPosition.y + 0.5F);
        }
        else
            _renderer.enabled = false;
    }
}