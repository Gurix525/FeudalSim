using Extensions;
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

    private static float _meshRotation;

    public static CursorMeshHighlight Instance { get; private set; }
    public static bool IsBlocked { get; set; } = false;

    public static void TrySetMesh(Mesh mesh)
    {
        if (Instance._mesh != mesh)
            Instance._mesh = mesh;
    }

    public static void SetMeshRotation(float rotation)
    {
        _meshRotation = rotation;
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
        if (_mesh != null && Cursor.ExactPosition != null)
        {
            _renderer.enabled = true;
            _renderer.material.renderQueue = 3001;
            var position = Cursor.ExactPosition.Value;
            var calibratedPosition = new Vector3(
                Mathf.Floor(position.x),
                Mathf.Round(position.y),
                Mathf.Floor(position.z));
            _highlight.transform.SetPositionAndRotation(
                calibratedPosition,
                Quaternion.Euler(0, _meshRotation, 0));
        }
        else
            _renderer.enabled = false;
    }
}