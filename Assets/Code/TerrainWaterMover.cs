using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainWaterMover : MonoBehaviour
{
    private void Awake()
    {
        TerrainGenerator.TerrainUpdating.AddListener(OnTerrainUpdating);
    }

    private void OnTerrainUpdating(Vector2 activeChunkPosition)
    {
        transform.position = new Vector3(
            activeChunkPosition.x * 100F + 50F,
            -0.1F,
            activeChunkPosition.y * 100F + 50F);
    }
}