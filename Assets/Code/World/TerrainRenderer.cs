using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Misc;
using PlayerControls;
using UI;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

namespace World
{
    public class TerrainRenderer : MonoBehaviour
    {
        #region Events

        public event EventHandler NavMeshRebaked;

        #endregion Events

        #region Fields

        [SerializeField] private WorldPopulator _worldPopulator;
        [SerializeField] private Material _material;

        private Chunk _activeChunk;
        private Dictionary<Vector2Int, ChunkRenderer> _chunks = new();
        private static TerrainRenderer _instance;
        private static float _timeSinceLastNavMeshRebuild = 0F;
        private static Vector3 _lastPlayerPosition;

        private static readonly int _maxChunkNumber = 6;

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
            yield return GenerateChunks(position);
            ActiveChunk = Terrain.Chunks[position];
            yield return _instance.StartCoroutine(Reload());
            LoadingImage.Disable();
        }

        public static IEnumerator GenerateWorld(Vector2Int startChunkPosition)
        {
            yield return GenerateChunks(startChunkPosition);
            _instance._worldPopulator.PopulateWorld();
            _instance.StartCoroutine(Reload());
        }

        public static void ReloadChunk(Vector2Int chunkPosition)
        {
            if (!IsChunkPositionValid(chunkPosition))
                return;
            _instance._chunks[chunkPosition].GenerateMesh();
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
            if (Vector3.Distance(Player.Current.transform.position, _lastPlayerPosition) > 5F)
            {
                MarkNavMeshToReload();
                _lastPlayerPosition = Player.Current.transform.position;
            }

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

        private static IEnumerator GenerateChunks(Vector2Int activePosition)
        {
            int iteration = 0;
            int mod = activePosition == Vector2Int.zero ? _maxChunkNumber : 0;
            for (int z = activePosition.y + mod - 1; z >= activePosition.y - mod; z--)
                for (int x = activePosition.x + mod - 1; x >= activePosition.x - mod; x--)
                    if (IsChunkPositionValid(new(x, z)))
                        if (!Terrain.Chunks.ContainsKey(new(x, z)))
                        {
                            LoadingScreen.OverwriteText($"Generating chunks:\n{iteration++} of {_maxChunkNumber * _maxChunkNumber * 4}");
                            Chunk chunk = new(new Vector2Int(x, z));
                            Terrain.Chunks.Add(new(x, z), chunk);
                            if (!_instance._chunks.ContainsKey(new(x, z)))
                                GenerateChunk(x, z);
                            yield return null;
                        }
            LoadingScreen.ClearText();

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
            NavMeshSurface.center = Player.Current.transform.position;
            NavMeshSurface.BuildNavMesh();
            _instance.NavMeshRebaked?.Invoke(_instance, EventArgs.Empty);
        }

        private static bool IsChunkPositionValid(Vector2Int position)
        {
            return position.x.IsInRangeInclusive(-_maxChunkNumber, _maxChunkNumber)
                && position.y.IsInRangeInclusive(-_maxChunkNumber, _maxChunkNumber);
        }

        #endregion Private
    }
}