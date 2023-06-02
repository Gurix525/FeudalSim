using System;
using System.Threading.Tasks;
using Extensions;
using UnityEngine;

namespace World
{
    public class GrassInstancer : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;

        private Matrix4x4[][] _batches = new Matrix4x4[10][];

        private float _timeSinceLastActivation = 0F;
        private Vector3 _lastPosition = Vector3.zero;
        private bool _isInitialized = false;
        private Vector2Int _currentPlayerPosition;

        public static bool HasToReload { get; private set; }

        public static void MarkToReload()
        {
            HasToReload = true;
        }

        private void Awake()
        {
            for (int i = 0; i < _batches.Length; i++)
                _batches[i] = new Matrix4x4[1000];
        }

        private void FixedUpdate()
        {
            _timeSinceLastActivation += Time.fixedDeltaTime;
            if ((_timeSinceLastActivation > 0.5F && Vector3.Distance(_player.position, _lastPosition) > 10F)
                || !_isInitialized
                || HasToReload)
            {
                _isInitialized = true;
                HasToReload = false;
                _timeSinceLastActivation = 0F;
                _lastPosition = _player.position;
                _currentPlayerPosition = new((int)Mathf.Floor(_player.position.x), (int)Mathf.Floor(_player.position.z));

                Task.Run(InstantiateGrass);
            }
        }

        private void Update()
        {
            RenderBatches();
        }

        private Task InstantiateGrass()
        {
            Matrix4x4[] positions = new Matrix4x4[3600];

            int index = 0;
            for (int z = _currentPlayerPosition.y - 29; z < _currentPlayerPosition.y + 30; z++)
                for (int x = _currentPlayerPosition.x - 29; x < _currentPlayerPosition.x + 30; x++)
                {
                    Cell cell = Terrain.GetCell(new Vector2Int(x, z));
                    if (!cell.HasGrass)
                    {
                        index += 1;
                        continue;
                    }
                    Vector3 position = new(
                        x,
                        Terrain.GetHeight(new(x, z)),
                        z);
                    float noise = NoiseSampler.GetNoise(position, -1F, 1F, 1000F);
                    float noise2 = Mathf.Abs((noise * 2) % 1F);
                    float noise3 = (noise2 * 3) % 1F;
                    float noise4 = (noise3 * 4) % 1F;
                    positions[index] = Matrix4x4.TRS(
                        position + new Vector3(0.5F, 0F, 0.5F) + new Vector3(0.1F, 0F, 0.1F) * noise,
                        Quaternion.Euler(new(0F, noise * 180F, 0F)),
                        new Vector3(
                            noise2.Remap(0F, 1F, 1.5F, 2F),
                            noise3.Remap(0F, 1F, 0.25F, 0.5F),
                            noise4.Remap(0F, 1F, 1.5F, 2F)));
                    index++;

                    //Vector3 position2 = position + new Vector3(0.5F, 0F, 0.5F);
                    //float noise5 = NoiseSampler.GetNoise(position2, -1F, 1F, 1000F);
                    //float noise6 = Mathf.Abs((noise5 * 2) % 1F);
                    //float noise7 = (noise6 * 3) % 1F;
                    //float noise8 = (noise7 * 4) % 1F;
                    //positions[index] = Matrix4x4.TRS(
                    //    position + new Vector3(0.5F, 0F, 0.5F) + new Vector3(0.1F, 0F, 0.1F) * noise + new Vector3(0.5F, 0F, 0.5F),
                    //    Quaternion.Euler(new(-90F, noise * 180F, 0F)),
                    //    new Vector3(
                    //        noise2.Remap(0F, 1F, 1.5F, 2F),
                    //        noise3.Remap(0F, 1F, 1.5F, 2F),
                    //        noise4.Remap(0F, 1F, 0.5F, 1F)));
                    //index++;
                }

            for (int i = 0; i < 3600; i++)
            {
                _batches[i / 1000][i % 1000] = positions[i];
            }
            return Task.CompletedTask;
        }

        private void RenderBatches()
        {
            for (int i = 0; i < _batches.Length; i++)
                Graphics.DrawMeshInstanced(_mesh, 0, _material, _batches[i]);
        }
    }
}