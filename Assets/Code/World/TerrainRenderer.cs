using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace World
{
    public class TerrainRenderer : MonoBehaviour
    {
        private Chunk _activeChunk;

        public static Chunk ActiveChunk
        {
            get => _instance._activeChunk;
            set => _instance._activeChunk = value;
        }

        private static TerrainRenderer _instance { get; set; }
        public static UnityEvent<Vector2> TerrainUpdating { get; private set; } = new();

        [SerializeField] private Material _material;

        private Dictionary<Vector2Int, ChunkRenderer> _chunks = new();

        public static IEnumerator SetActiveChunk(Vector2Int position)
        {
            LoadingImage.Enable();
            yield return null;
            GenerateChunks(position);
            ActiveChunk = Terrain.Chunks[position];
            Reload();
            LoadingImage.Disable();
        }

        public static void ReloadChunk(Vector2Int position)
        {
            _instance._chunks[position].GenerateMesh();
            TerrainUpdating.Invoke(ActiveChunk.Position);
        }

        public static void Reload()
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
        }

        public static ChunkRenderer GetChunkRenderer(Vector2Int position)
        {
            return _instance._chunks[Terrain.GetChunkCoordinates(position)];
        }

        public static ChunkRenderer GetChunkRenderer(Vector3 position)
        {
            return GetChunkRenderer(new Vector2Int((int)position.x, (int)position.z));
        }

        private void Awake()
        {
            _instance = this;
            GenerateChunks(Vector2Int.zero);
            Reload();
        }

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
                        Terrain.Chunks.Add(new(x, z), new(new(x, z)));

            for (int z = activePosition.y - 1; z <= activePosition.y + 1; z++)
                for (int x = activePosition.x - 1; x <= activePosition.x + 1; x++)
                    if (!_instance._chunks.ContainsKey(new(x, z)))
                        GenerateChunk(x, z);

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
    }
}