using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class TerrainChunkActivator : MonoBehaviour
    {
        private float _timeSinceLastActivation = 0F;

        private void FixedUpdate()
        {
            _timeSinceLastActivation += Time.fixedDeltaTime;
            if (_timeSinceLastActivation > 0.5F)
                ActivateChunk();
        }

        private void ActivateChunk()
        {
            Vector2Int currentChunkPos = new((int)Mathf.Floor(transform.position.x / 100F), (int)Mathf.Floor(transform.position.z / 100F));
            if (TerrainRenderer.Instance.ActiveChunk.Position != currentChunkPos)
            {
                _timeSinceLastActivation = 0F;
                StartCoroutine(TerrainRenderer.SetActiveChunk(currentChunkPos));
            }
        }
    }
}