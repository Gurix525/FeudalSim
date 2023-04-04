using UnityEngine;
using Cursor = Controls.Cursor;
using Terrain = World.Terrain;

public class BuildingHighlight : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _notBlockedMaterial;
    [SerializeField] private Material _blockedMaterial;

    private Material _currentMaterial;

    private GameObject _highlight;
    private MeshFilter _filter;
    private MeshRenderer _renderer;

    public void SetBlocked(bool state)
    {
        if (state)
            _currentMaterial = _blockedMaterial;
        else
            _currentMaterial = _notBlockedMaterial;
    }

    private void Awake()
    {
        _highlight = new GameObject();
        _filter = _highlight.AddComponent<MeshFilter>();
        _renderer = _highlight.AddComponent<MeshRenderer>();
        _currentMaterial = _notBlockedMaterial;
        _renderer.material = _currentMaterial;
    }

    private void Update()
    {
        if (_mesh != _filter.mesh)
            _filter.mesh = _mesh;
        if (_renderer.material != _currentMaterial)
            _renderer.material = _currentMaterial;
        if (Cursor.CellPosition != null && _mesh != null)
        {
            var cellPosition = Cursor.CellPosition.Value;
            _highlight.transform.position = new(
                cellPosition.x + 0.5F, Terrain.GetHeight(cellPosition), cellPosition.y + 0.5F);
        }
    }
}