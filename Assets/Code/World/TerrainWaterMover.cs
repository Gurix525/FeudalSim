using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class TerrainWaterMover : MonoBehaviour
    {
        private void Awake()
        {
            TerrainRenderer.TerrainUpdating.AddListener(OnTerrainUpdating);
        }

        private void OnTerrainUpdating(Vector2 activeChunkPosition)
        {
            transform.position = new Vector3(
                activeChunkPosition.x * 100F + 50F,
                -0.1F,
                activeChunkPosition.y * 100F + 50F);
        }
    }
}