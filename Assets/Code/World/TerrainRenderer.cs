using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
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

        #endregion Fields

        #region Properties

        public static Chunk ActiveChunk
        {
            get => Instance._activeChunk;
            set => Instance._activeChunk = value;
        }

        private static TerrainRenderer Instance =>
            _instance ??= FindObjectOfType<TerrainRenderer>();

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
            yield return Instance.StartCoroutine(Reload());
            LoadingImage.Disable();
        }

        public static void GenerateWorld(Vector2Int startChunkPosition)
        {
            GenerateChunks(startChunkPosition);
            Instance.StartCoroutine(Reload());
        }

        public static void ReloadChunk(Vector2Int position)
        {
            Instance._chunks[position].GenerateMesh();
            TerrainUpdating.Invoke(ActiveChunk.Position);
        }

        public static IEnumerator Reload()
        {
            foreach (var chunk in Instance._chunks)
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
            Instance._chunks.TryGetValue(
                Terrain.GetChunkCoordinates(chunkPosition), out ChunkRenderer renderer);
            return renderer;
        }

        public static ChunkRenderer GetChunkRenderer(Vector3 position)
        {
            return GetChunkRenderer(new Vector2Int((int)position.x, (int)position.z));
        }

        public static ChunkRenderer GetChunkRenderer(Chunk chunk)
        {
            Instance._chunks.TryGetValue(chunk.Position, out ChunkRenderer renderer);
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
            InitializeNavMesh();
        }

        private void FixedUpdate()
        {
            _timeSinceLastNavMeshRebuild += Time.fixedDeltaTime;
            if (NavMeshHasToRebuild && _timeSinceLastNavMeshRebuild > 0.5F)
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
            int mod = activePosition == Vector2Int.zero ? 2 : 0;
            for (int z = activePosition.y - 1 - mod; z <= activePosition.y + 2 + mod; z++)
                for (int x = activePosition.x - 1 - mod; x <= activePosition.x + 2 + mod; x++)
                    if (!Terrain.Chunks.ContainsKey(new(x, z)))
                        Terrain.Chunks.Add(new(x, z), new(new Vector2Int(x, z)));

            for (int z = activePosition.y - 1; z <= activePosition.y + 1; z++)
                for (int x = activePosition.x - 1; x <= activePosition.x + 1; x++)
                    if (!Instance._chunks.ContainsKey(new(x, z)))
                        GenerateChunk(x, z);

            foreach (var chunk in Terrain.Chunks.Values)
            {
                if (chunk.IsNatureSpawned && !Instance._chunks
                    .ContainsKey(new(chunk.Position.x, chunk.Position.y)))
                    GenerateChunk(chunk.Position.x, chunk.Position.y);
            }

            ActiveChunk ??= Terrain.Chunks[activePosition];
        }

        private static void GenerateChunk(int x, int z)
        {
            GameObject chunk = new GameObject();
            chunk.transform.parent = Instance.transform;
            chunk.gameObject.name = new Vector2Int(x, z).ToString();
            chunk.AddComponent<ChunkRenderer>();
            chunk.GetComponent<MeshRenderer>().material = Instance._material;
            var chunkRenderer = chunk.GetComponent<ChunkRenderer>();
            chunkRenderer.SetPosition(new(x, z));
            Instance._chunks.Add(new(x, z), chunkRenderer);
            chunkRenderer.GenerateMesh();
        }

        private static void InitializeNavMesh()
        {
            NavMeshSurface = Instance.GetComponent<NavMeshSurface>();
        }

        private static void RebuildNavMesh()
        {
            _timeSinceLastNavMeshRebuild = 0F;
            NavMeshHasToRebuild = false;
            DateTime t1 = DateTime.Now;
            NavMeshSurface.BuildNavMesh();
            DateTime t2 = DateTime.Now;
            Debug.Log((t2 - t1).TotalSeconds);
        }

        #endregion Private
    }
}