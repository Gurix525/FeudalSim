using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassInstancer : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;

    private TerrainGenerator _terrainGenerator;
    private Matrix4x4[][] _batches = new Matrix4x4[180][];

    private void Awake()
    {
        for (int i = 0; i < _batches.Length; i++)
            _batches[i] = new Matrix4x4[1000];
        _terrainGenerator = GetComponent<TerrainGenerator>();
    }

    private void Start()
    {
        for (int i = 0; i < _batches.Length / 2; i++)
            for (int j = 0; j < 1000; j++)
            {
                Vector3 position = _terrainGenerator.Vertices[i * 1000 + j];
                float noise = NoiseSampler.GetNoise(position, -1F, 1F, 1000F);
                float noise2 = Mathf.Abs((noise * 2) % 1F);
                float noise3 = (noise2 * 2) % 1F;
                float noise4 = (noise3 * 2) % 1F;
                _batches[i][j] = Matrix4x4.TRS(
                    position + new Vector3(0.1F, 0F, 0.1F) * noise,
                    Quaternion.Euler(new(-90F, noise * 180F, 0F)),
                    new Vector3(
                        noise2.Remap(0F, 1F, 1F, 1.5F),
                        noise3.Remap(0F, 1F, 1F, 1.5F),
                        noise4.Remap(0F, 1F, 0.5F, 1F)));
            }

        for (int i = 0; i < _batches.Length / 2; i++)
            for (int j = 0; j < 1000; j++)
            {
                Vector3 position = _terrainGenerator.Vertices[i * 1000 + j];
                float noise = NoiseSampler.GetNoise(position, -1F, 1F, 1000F);
                float noise2 = Mathf.Abs((noise * 2) % 1F);
                float noise3 = (noise2 * 2) % 1F;
                float noise4 = (noise3 * 2) % 1F;
                _batches[90 + i][j] = Matrix4x4.TRS(
                    position + new Vector3(0.1F, 0F, 0.1F) * noise + new Vector3(0.5F, 0F, 0.5F),
                    Quaternion.Euler(new(-90F, noise * 180F, 0F)),
                    new Vector3(
                        noise2.Remap(0F, 1F, 1F, 1.5F),
                        noise3.Remap(0F, 1F, 1F, 1.5F),
                        noise4.Remap(0F, 1F, 0.5F, 1F)));
            }
    }

    private void Update()
    {
        RenderBatches();
    }

    private void RenderBatches()
    {
        for (int i = 0; i < _batches.Length; i++)
            Graphics.DrawMeshInstanced(_mesh, 0, _material, _batches[i]);
    }
}