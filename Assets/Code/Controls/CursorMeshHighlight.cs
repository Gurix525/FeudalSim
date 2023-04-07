using Controls;
using Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private BuildingMode _buildingMode = BuildingMode.BigWall;
    private Vector3 _previousPosition = Vector3.zero;

    private static float _meshRotation;

    public static bool IsBlocked { get; set; } = false;
    private static CursorMeshHighlight _instance { get; set; }

    public static void TrySetMesh(Mesh mesh)
    {
        if (_instance._mesh != mesh)
        {
            _instance._mesh = mesh;
            _instance._previousPosition = Vector3.zero;
        }
    }

    public static void SetBuildingMode(BuildingMode buildingMode)
    {
        _instance._buildingMode = buildingMode;
    }

    public static void SetMeshRotation(float rotation)
    {
        _meshRotation = rotation;
    }

    private void Awake()
    {
        _instance = this;
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
        if (_mesh != null && Cursor.RaycastHit != null)
        {
            _renderer.enabled = true;
            _renderer.material.renderQueue = 3001;
            var position = Cursor.RaycastHit.Value.point;
            var calibratedPosition = new Vector3(
                Mathf.Floor(position.x),
                Mathf.Round(position.y),
                Mathf.Floor(position.z));
            _highlight.transform.SetPositionAndRotation(
                calibratedPosition,
                Quaternion.Euler(0, _meshRotation, 0));
            if (calibratedPosition != _previousPosition)
            {
                _previousPosition = calibratedPosition;
                bool isBuildingPossible = Terrain.IsBuildingPossible(
                    calibratedPosition.ToVector3Int(),
                    _buildingMode,
                    _meshRotation);
                if (isBuildingPossible)
                    IsBlocked = false;
                else
                    IsBlocked = true;
            }
        }
        else
            _renderer.enabled = false;
    }
}