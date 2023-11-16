using System.Collections;
using System.Collections.Generic;
using Extensions;
using Misc;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

namespace World
{
    public class TerrainRenderer : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Material _material;

        private Chunk _activeChunk;
        private Dictionary<Vector2Int, ChunkRenderer> _chunks = new();
        private static TerrainRenderer _instance;
        private static float _timeSinceLastNavMeshRebuild = 0F;

        private static readonly int _minChunkNumber = -9;
        private static readonly int _maxChunkNumber = 10;

        #endregion Fields

        #region Properties

        public static Chunk ActiveChunk
        {
            get => _instance._activeChunk;
            set => _instance._activeChunk = value;
        }

        public static UnityEvent<Vector2> TerrainUpdating { get; private set; } = new();

        public static NavMeshSurface NavMeshSurface { get; private set; }

        public static bool NavMeshHasToRebuild { get; private set; }

        #endregion Properties

        #region Public

        public static IEnumerator SetActiveChunk(Vector2Int position)
        {
            LoadingImage.Enable();
            yield return null;
            GenerateChunks(position);
            ActiveChunk = Terrain.Chunks[position];
            yield return _instance.StartCoroutine(Reload());
            LoadingImage.Disable();
        }

        public static void GenerateWorld(Vector2Int startChunkPosition)
        {
            GenerateChunks(startChunkPosition);
            _instance.StartCoroutine(Reload());
        }

        public static void ReloadChunk(Vector2Int position)
        {
            if (!IsChunkPositionValid(position))
                return;
            _instance._chunks[position].GenerateMesh();
            TerrainUpdating.Invoke(ActiveChunk.Position);
        }

        public static IEnumerator Reload()
        {
            foreach (var chunk in _instance._chunks)
            {
                if (Vector2Int.Distance(chunk.Value.Position, ActiveChunk.Position) < 2F)
                {
                    chunk.Value.gameObject.SetActive(true);
                }
                else
                    chunk.Value.gameObject.SetActive(false);
            }
            TerrainUpdating.Invoke(ActiveChunk.Position);
            RecalculateActiveChunkBorderSteepness();
            yield return null;
            MarkNavMeshToReload();
        }

        public static ChunkRenderer GetChunkRenderer(Vector2Int chunkPosition)
        {
            _instance._chunks.TryGetValue(
                Terrain.GetChunkCoordinates(chunkPosition),
                out ChunkRenderer renderer);
            return renderer;
        }

        public static ChunkRenderer GetChunkRenderer(Vector3 position)
        {
            return GetChunkRenderer(new Vector2Int((int)position.x, (int)position.z));
        }

        public static ChunkRenderer GetChunkRenderer(Chunk chunk)
        {
            _instance._chunks.TryGetValue(chunk.Position, out ChunkRenderer renderer);
            return renderer;
        }

        public static void MarkNavMeshToReload()
        {
            NavMeshHasToRebuild = true;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _instance = this;
            InitializeNavMesh();
        }

        private void FixedUpdate()
        {
            _timeSinceLastNavMeshRebuild += Time.fixedDeltaTime;
            if (NavMeshHasToRebuild && _timeSinceLastNavMeshRebuild > 1F)
                RebuildNavMesh();
        }

        #endregion Unity

        #region Private

        private static void RecalculateActiveChunkBorderSteepness()
        {
            Terrain.Chunks[ActiveChunk.Position].RecalculateBorderSteepness();
        }

        private static void GenerateChunks(Vector2Int activePosition)
        {
            int mod = activePosition == Vector2Int.zero ? 12 : 0;
            for (int z = activePosition.y - 1 - mod; z <= activePosition.y + 2 + mod; z++)
                for (int x = activePosition.x - 1 - mod; x <= activePosition.x + 2 + mod; x++)
                    if (IsChunkPositionValid(new(x, z)))
                        if (!Terrain.Chunks.ContainsKey(new(x, z)))
                            Terrain.Chunks.Add(new(x, z), new(new Vector2Int(x, z)));

            for (int z = activePosition.y - 1; z <= activePosition.y + 1; z++)
                for (int x = activePosition.x - 1; x <= activePosition.x + 1; x++)
                    if (!_instance._chunks.ContainsKey(new(x, z)))
                        if (IsChunkPositionValid(new(x, z)))
                            GenerateChunk(x, z);

            foreach (var chunk in Terrain.Chunks.Values)
            {
                if (chunk.IsNatureSpawned && !_instance._chunks
                    .ContainsKey(new(chunk.Position.x, chunk.Position.y)))
                    GenerateChunk(chunk.Position.x, chunk.Position.y);
            }

            ActiveChunk ??= Terrain.Chunks[activePosition];
        }

        private static void GenerateChunk(int x, int z)
        {
            GameObject chunk = new GameObject();
            chunk.transform.parent = _instance.transform;
            chunk.gameObject.name = new Vector2Int(x, z).ToString();
            chunk.AddComponent<ChunkRenderer>();
            chunk.GetComponent<MeshRenderer>().material = _instance._material;
            var chunkRenderer = chunk.GetComponent<ChunkRenderer>();
            chunkRenderer.SetPosition(new(x, z));
            _instance._chunks.Add(new(x, z), chunkRenderer);
            chunkRenderer.GenerateMesh();
        }

        private static void InitializeNavMesh()
        {
            NavMeshSurface = _instance.GetComponent<NavMeshSurface>();
        }

        private static void RebuildNavMesh()
        {
            _timeSinceLastNavMeshRebuild = 0F;
            NavMeshHasToRebuild = false;
            NavMeshSurface.BuildNavMesh();
        }

        private static bool IsChunkPositionValid(Vector2Int position)
        {
            return position.x.IsInRangeInclusive(_minChunkNumber, _maxChunkNumber)
                && position.y.IsInRangeInclusive(_minChunkNumber, _maxChunkNumber);
        }

        #endregion Private
    }
}