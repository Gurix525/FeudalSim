using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int _size;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _detailScale;
    [SerializeField] private int _octaves;
    [SerializeField] private float _frequencyFactor;
    [SerializeField] private float _amplitudeFactor;

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        Mesh mesh = GenerateMesh();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    private void OnValidate()
    {
        Mesh mesh = GenerateMesh();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    private Mesh GenerateMesh()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();
        System.Random random = new();

        for (int z = 0; z < _size + 1; z++)
            for (int x = 0; x < _size + 1; x++)
            {
                vertices.Add(new Vector3(x, GetPerlinHeight(x, z, _octaves), z));
            }

        for (int z = 0; z < _size; z++)
            for (int x = 0; x < _size; x++)
            {
                int i = (z * _size) + z + x;

                triangles.Add(i);
                triangles.Add(i + _size + 1);
                triangles.Add(i + _size + 2);
                triangles.Add(i);
                triangles.Add(i + _size + 2);
                triangles.Add(i + 1);
            }

        Mesh mesh = new();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        //List<Vector2> uvs = new();
        //foreach (var vertice in vertices)
        //    uvs.Add(new(vertice.x, vertice.z));
        //mesh.uv = uvs.ToArray();

        mesh.uv = vertices
            .Select(x => new Vector2(x.x, x.z))
            .ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }

    private float GetPerlinHeight(int x, int z, int octaves)
    {
        float result = 0F;
        float resultDivider = 0F;
        for (int i = 0; i < octaves; i++)
        {
            result += Mathf.PerlinNoise(
                x * _detailScale * Mathf.Pow(_frequencyFactor, i),
                z * _detailScale * Mathf.Pow(_frequencyFactor, i))
                / Mathf.Pow(_amplitudeFactor, i);
            resultDivider += 1F / Mathf.Pow(_amplitudeFactor, i);
        }
        result /= resultDivider;
        float remapped = result.Remap(0.4F, 0.6F, 0F, _maxHeight);
        return Mathf.Round(remapped * 2F) / 2F;
    }
}

public static class FloatExtensions
{
    public static float Remap(this float value, float startA, float startB, float targetA, float targetB)
    {
        return (value - startA) / (startB - startA) * (targetB - targetA) + targetA;
    }
}