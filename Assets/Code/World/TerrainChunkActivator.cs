using UnityEngine;

namespace World
{
    public class TerrainChunkActivator : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private float _timeSinceLastActivation = 0F;

        private void FixedUpdate()
        {
            _timeSinceLastActivation += Time.fixedDeltaTime;
            if (_timeSinceLastActivation > 0.5F)
                ActivateChunk();
        }

        private void ActivateChunk()
        {
            if (TerrainRenderer.ActiveChunk == null)
                return;
            Vector2Int currentChunkPos = new((int)Mathf.Floor(_player.position.x / 50F), (int)Mathf.Floor(_player.position.z / 50F));
            if (TerrainRenderer.ActiveChunk.Position != currentChunkPos)
            {
                _timeSinceLastActivation = 0F;
                StartCoroutine(TerrainRenderer.SetActiveChunk(currentChunkPos));
            }
        }
    }
}