using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

public class TerrainRenderer : MonoBehaviour
{
    public Chunk ActiveChunk { get; private set; }

    public static TerrainRenderer Instance { get; private set; }
    public static UnityEvent<Vector2> TerrainUpdating { get; private set; } = new();

    [SerializeField] private Material _material;

    private Dictionary<Vector2Int, ChunkRenderer> _chunks = new();

    public static void SetActiveChunk(Vector2Int position)
    {
        GenerateChunks(position);
        Instance.ActiveChunk = Terrain.Chunks[position];
        Reload();
    }

    public static void Reload()
    {
        Profiler.BeginSample("Reload");
        foreach (var chunk in Instance._chunks)
        {
            if (Vector2Int.Distance(chunk.Value.Position, Instance.ActiveChunk.Position) < 2F)
            {
                chunk.Value.gameObject.SetActive(true);
            }
            else
                chunk.Value.gameObject.SetActive(false);
        }
        //foreach (var chunk in Instance._chunks.Values)
        //{
        //    chunk.GenerateMesh();
        //}
        TerrainUpdating.Invoke(Instance.ActiveChunk.Position);
        RecalculateActiveChunkBorderSteepness();
        Profiler.EndSample();
    }

    private void Awake()
    {
        Instance = this;
        GenerateChunks(Vector2Int.zero);
        Reload();
    }

    private static void RecalculateActiveChunkBorderSteepness()
    {
        Terrain.Chunks[Instance.ActiveChunk.Position].RecalculateBorderSteepness();
    }

    private static void GenerateChunks(Vector2Int activePosition)
    {
        Profiler.BeginSample("GenerateChunks");
        for (int x = activePosition.x - 1; x <= activePosition.x + 1; x++)
            for (int z = activePosition.y - 1; z <= activePosition.y + 1; z++)
            {
                if (!Terrain.Chunks.ContainsKey(new(x, z)))
                {
                    Terrain.Chunks.Add(new(x, z), new(new(x, z)));
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
            }
        Instance.ActiveChunk ??= Terrain.Chunks[activePosition];
        Profiler.EndSample();
    }
}