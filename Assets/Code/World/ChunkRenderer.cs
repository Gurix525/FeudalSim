using System.Linq;
using Misc;
using UnityEngine;

namespace World
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    public class ChunkRenderer : MonoBehaviour
    {
        #region Fields

        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private Mesh _mesh;
        private bool _isInitialized = false;

        #endregion Fields

        #region Properties

        public Vector2Int Position { get; private set; }
        public Transform Buildings { get; private set; }
        public Transform ItemHandlers { get; private set; }
        public Transform Trees { get; private set; }
        public Transform Boulders { get; private set; }

        #endregion Properties

        #region Public

        public void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        public void SetColors()
        {
            Color[] colors = new Color[2601];
            var thisChunk = Terrain.Chunks[Position].Colors;
            var rightChunk = Terrain.Chunks[new(Position.x + 1, Position.y)].Colors;
            var upChunk = Terrain.Chunks[new(Position.x, Position.y + 1)].Colors;
            var diagonalChunk = Terrain.Chunks[new(Position.x + 1, Position.y + 1)].Colors;
            for (int z = 0; z < 51; z++)
            {
                for (int x = 0; x < 51; x++)
                {
                    if (z < 50 && x < 50)
                        colors[z * 51 + x] = thisChunk[z * 50 + x];
                    else if (x == 50 && z < 50)
                        colors[z * 51 + x] = rightChunk[z * 50];
                    else if (z == 50 && x < 50)
                        colors[z * 51 + x] = upChunk[x];
                    else
                        colors[2600] = diagonalChunk[0];
                }
            }
            _mesh.SetColors(colors);
        }

        public void GenerateMesh()
        {
            if (!_isInitialized)
                Initialize();

            //int size = 101;

            Vector3[] vertices = new Vector3[2601];
            var thisChunk = Terrain.Chunks[Position].Vertices;
            var rightChunk = Terrain.Chunks[new(Position.x + 1, Position.y)].Vertices;
            var upChunk = Terrain.Chunks[new(Position.x, Position.y + 1)].Vertices;
            var diagonalChunk = Terrain.Chunks[new(Position.x + 1, Position.y + 1)].Vertices;
            for (int z = 0; z < 51; z++)
            {
                for (int x = 0; x < 51; x++)
                {
                    if (z < 50 && x < 50)
                        vertices[z * 51 + x] = thisChunk[z * 50 + x];
                    else if (x == 50 && z < 50)
                        vertices[z * 51 + x] = rightChunk[z * 50];
                    else if (z == 50 && x < 50)
                        vertices[z * 51 + x] = upChunk[x];
                    else
                        vertices[2600] = diagonalChunk[0];
                }
            }
            int[] triangles = new int[15000];

            int index = 0;
            for (int z = 0; z < 50; z++)
                for (int x = 0; x < 50; x++)
                {
                    int i = (z * 51) + x;

                    triangles[index] = i;
                    triangles[index + 1] = i + 51;
                    triangles[index + 2] = i + 52;
                    triangles[index + 3] = i;
                    triangles[index + 4] = i + 52;
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
            _meshCollider.sharedMesh = _mesh;
        }

        #endregion Public

        #region Private

        private void Initialize()
        {
            _isInitialized = true;
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
            CreateChildren();
            InitializeMesh();
            SpawnNature();
        }

        private void CreateChildren()
        {
            Buildings = new GameObject("Buildings").transform;
            Buildings.transform.parent = transform;

            ItemHandlers = new GameObject("ItemHandlers").transform;
            ItemHandlers.transform.parent = transform;

            Boulders = new GameObject("Boulders").transform;
            Boulders.transform.parent = transform;

            Trees = new GameObject("Trees").transform;
            Trees.transform.parent = transform;
        }

        private void InitializeMesh()
        {
            _mesh = new();
            _mesh.MarkDynamic();
            _meshFilter.mesh = _mesh;
            _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }

        private void SpawnNature()
        {
            Chunk chunk = Terrain.Chunks[Position];
            if (chunk.IsNatureSpawned)
                return;
            chunk.IsNatureSpawned = true;
            SpawnTrees();
            SpawnBoulders();
        }

        private void SpawnTrees()
        {
            var treePrefab = Resources.Load<GameObject>("Prefabs/Nature/Tree");
            for (int z = 0; z < 50; z++)
                for (int x = 0; x < 50; x++)
                {
                    Vector2 position = new Vector2(Position.x * 50 + x, Position.y * 50 + z);
                    float noise = NoiseSampler.GetTreesNoise(position.x, position.y);
                    if (noise == 1F)
                    {
                        float height = Terrain.GetHeight(position);
                        //if (height > 0 && height <= 6)
                        if (height > 0)
                        {
                            var tree = Instantiate(treePrefab, new Vector3(
                                position.x, height, position.y), Quaternion.identity, Trees);
                            tree.transform.position += RandomVector3.One / 4F;
                            tree.transform.localScale = new RandomVector3(0.8F, 1.2F);
                            tree.transform.rotation = Quaternion.Euler(new RandomVector3(0F, 60F, 0F));
                        }
                    }
                }
        }

        private void SpawnBoulders()
        {
            var boulderPrefab = Resources.Load<GameObject>("Prefabs/Nature/Boulder");
            for (int z = 0; z < 50; z++)
                for (int x = 0; x < 50; x++)
                {
                    Vector2 position = new Vector2(Position.x * 50 + x, Position.y * 50 + z);
                    float noise = NoiseSampler.GetBouldersNoise(position.x, position.y);
                    if (noise == 1F)
                    {
                        float height = Terrain.GetHeight(position);
                        var boulder = Instantiate(boulderPrefab, new Vector3(
                            position.x, height, position.y), Quaternion.identity, Boulders);
                        boulder.transform.position += RandomVector3.OneUnsigned / 4F;
                        boulder.transform.localScale = new RandomVector3(0.8F, 1.2F);
                        boulder.transform.rotation = Quaternion.Euler(new RandomVector3(0F, 360F, 0F));
                    }
                }
        }

        #endregion Private
    }
}