using System.Collections.Generic;
using UnityEngine;

public static class Terrain
{
    public static Dictionary<Vector2Int, Chunk> Chunks { get; } = new();
}