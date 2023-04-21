using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Input;
using UnityEngine;
using UnityEngine.UIElements;
using World;
using static UnityEngine.InputSystem.InputAction;
using Random = System.Random;

public class NoiseVisualisation : MonoBehaviour
{
    [SerializeField] private float _bigPools;
    [SerializeField] private float _smallPools1;
    [SerializeField] private float _smallPools2;
    [SerializeField] private float _smallPools3;

    [Header("")]
    [SerializeField] private float _bigPoolsStrength;

    [SerializeField] private float _smallPoolsStrength1;
    [SerializeField] private float _smallPoolsStrength2;
    [SerializeField] private float _smallPoolsStrength3;

    [Header("")]
    [SerializeField] private float _randomModifier;

    private MeshFilter _filter;

    private Mesh _mesh;

    public void GenerateMesh(CallbackContext context)
    {
        Vector3[] vertices = new Vector3[10201];
        for (int z = 0; z < 101; z++)
        {
            for (int x = 0; x < 101; x++)
            {
                vertices[z * 101 + x] = new Vector3(x, 0F, z);
            }
        }

        int[] triangles = new int[60000];
        int index = 0;
        for (int z = 0; z < 100; z++)
            for (int x = 0; x < 100; x++)
            {
                int i = (z * 101) + x;

                triangles[index] = i;
                triangles[index + 1] = i + 101;
                triangles[index + 2] = i + 102;
                triangles[index + 3] = i;
                triangles[index + 4] = i + 102;
                triangles[index + 5] = i + 1;
                index += 6;
            }
        _mesh.Clear();
        _mesh.SetVertices(vertices);
        SetColors();
        _mesh.SetTriangles(triangles, 0);
        _mesh.SetUVs(0, vertices
            .Select(x => new Vector2(x.x, x.z))
            .ToArray());
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
    }

    private void SetColors()
    {
        Color[] colors = new Color[10201];
        for (int z = 0; z < 101; z++)
        {
            for (int x = 0; x < 101; x++)
            {
                float noise = GetTreesNoise(x, z);
                colors[z * 101 + x] = new Color(noise, noise, noise);
            }
        }
        _mesh.SetColors(colors);
    }

    private float GetTreesNoise(float x, float z)
    {
        float bigPools = NoiseSampler.GetNoise(x, z, detailScale: _bigPools) * _bigPoolsStrength;
        float smallPools1 = NoiseSampler.GetNoise(x, z, detailScale: _smallPools1) * _smallPoolsStrength1;
        float smallPools2 = NoiseSampler.GetNoise(x, z, detailScale: _smallPools2) * _smallPoolsStrength2;
        float smallPools3 = NoiseSampler.GetNoise(x, z, detailScale: _smallPools3) * _smallPoolsStrength3;

        float sum = bigPools + smallPools1 + smallPools2 + smallPools3;
        float maxStrength = _bigPoolsStrength + _smallPoolsStrength1
            + _smallPoolsStrength2 + _smallPoolsStrength3;
        float remapped = sum.Remap(0F, maxStrength, 0F, 1F);

        Random random = new();
        return (float)random.NextDouble() + _randomModifier > remapped ? 0 : 1;
    }

    private void OnValidate()
    {
        _filter = GetComponent<MeshFilter>();
        _mesh = new();
        _filter.sharedMesh = _mesh;
        _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GenerateMesh(new());
    }
}