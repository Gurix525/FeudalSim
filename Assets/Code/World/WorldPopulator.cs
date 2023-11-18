using System.Collections.Generic;
using AI;
using Misc;
using UnityEngine;

namespace World
{
    public class WorldPopulator : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnerPrefab;
        [SerializeField] private List<WorldPopulatorItem> _items;

        public void PopulateWorld()
        {
            foreach (var item in _items)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    Vector3 spawnerPosition = GetRandomSpawnerPosition();
                    ChunkRenderer chunk = GetChunkRenderer(spawnerPosition);
                    if (chunk == null)
                        continue;
                    GameObject spawner = Instantiate(_spawnerPrefab, spawnerPosition, Quaternion.identity, chunk.Spawners);
                    var spawnerComponent = spawner.GetComponent<Spawner>();
                    spawnerComponent.Initialize(item.SpawnerModel);
                    spawnerComponent.SpawnAll();
                }
            }
        }

        private ChunkRenderer GetChunkRenderer(Vector3 spawnerPosition)
        {
            return TerrainRenderer.GetChunkRenderer(spawnerPosition);
        }

        private Vector3 GetRandomSpawnerPosition()
        {
            float radius = NoiseSampler.IslandRadius;
            Vector3 randomPosition = new RandomVector3(-radius, radius, 0F, 5F, -radius, radius);
            float height = Terrain.GetHeight(randomPosition.x, randomPosition.z);
            return new(randomPosition.x, height, randomPosition.z);
        }
    }
}