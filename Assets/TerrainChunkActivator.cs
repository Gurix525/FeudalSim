using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunkActivator : MonoBehaviour
{
    private void FixedUpdate()
    {
        ActivateChunk();
    }

    private void ActivateChunk()
    {
        Vector2Int currentChunkPos = new(((int)Math.Floor(transform.position.x)) / 100, ((int)Math.Floor(transform.position.z)) / 100);
        TerrainGenerator.ChangeActiveChunk(currentChunkPos);
    }
}