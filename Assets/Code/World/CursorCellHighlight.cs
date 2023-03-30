using System.Linq;
using UnityEngine;

namespace World
{
    public class CursorCellHighlight : MonoBehaviour
    {
        [SerializeField] private Material _material;

        private GameObject _highlight;
        private MeshFilter _meshFilter;
        private MeshRenderer _renderer;
        private Mesh _mesh;
        private Vector2 _lastPosition;

        private void Awake()
        {
            _highlight = new GameObject();
            _highlight.name = $"TerrainHighlight";
            _meshFilter = _highlight.AddComponent<MeshFilter>();
            _renderer = _highlight.AddComponent<MeshRenderer>();
            _renderer.material = _material;
            InitializeMesh();
        }

        private void OnEnable()
        {
            TerrainRenderer.TerrainUpdating.AddListener(OnTerrainUpdating);
        }

        private void Update()
        {
            if (Controls.Cursor.IsAboveTerrain)
            {
                _highlight.SetActive(true);
                UpdateHighlight();
            }
            else
                _highlight.SetActive(false);
        }

        private void OnDisable()
        {
            TerrainRenderer.TerrainUpdating.RemoveListener(OnTerrainUpdating);
        }

        private void OnTerrainUpdating(Vector2 activeChunkPosition)
        {
            _lastPosition = Vector2.zero;
        }

        private void InitializeMesh()
        {
            _mesh = new Mesh();
            Vector3[] vertices = new Vector3[4]
            {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1)
            };
            _mesh.MarkDynamic();
            _meshFilter.mesh = _mesh;
            _mesh.SetVertices(vertices);
        }

        private void UpdateHighlight()
        {
            _material.renderQueue = 3001;
            Vector2 cursorPosition = (Vector2)Controls.Cursor.CellPosition;
            if (cursorPosition == _lastPosition)
                return;
            _lastPosition = cursorPosition;
            float x = cursorPosition.x;
            float z = cursorPosition.y;
            Vector3[] vertices = new Vector3[4]
            {
            new(x, Terrain.GetHeight(new(x, z)), z),
            new(x + 1, Terrain.GetHeight(new(x + 1, z)), z),
            new(x, Terrain.GetHeight(new(x, z + 1)), z + 1),
            new(x + 1, Terrain.GetHeight(new(x + 1, z + 1)), z + 1)
            };
            int[] indices = new int[6] { 0, 2, 3, 0, 3, 1 };
            _mesh.SetVertices(vertices);
            _mesh.SetTriangles(indices, 0);
            _mesh.SetUVs(0, vertices
                .Select(x => new Vector2(x.x, x.z))
                .ToArray());
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
            _mesh.RecalculateTangents();
        }
    }
}