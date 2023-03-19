using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Terrain
{
    public static Dictionary<Vector2Int, Chunk> Chunks { get; } = new();

    public static Cell GetCell(Vector3 inputPosition)
    {
        return GetCell(new Vector2(inputPosition.x, inputPosition.z));
    }

    public static Cell GetCell(Vector2 inputPosition)
    {
        Vector2Int chunkPosition = Vector2Int.zero;
        Vector2Int cellPosition = GetVerticeCoordinates(inputPosition);
        try
        {
            chunkPosition = GetChunkCoordinates(inputPosition);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
        return Chunks[chunkPosition][cellPosition];
    }

    public static float GetHeight(Vector2 inputPosition)
    {
        return GetCell(inputPosition).Height;
    }

    public static float GetSteepness(Vector2 inputPosition)
    {
        return GetCell(inputPosition).Steepness;
    }

    private static Vector2Int GetChunkCoordinates(Vector2 inputPosition)
    {
        Vector2Int position = new(
            (int)Mathf.Floor(inputPosition.x),
            (int)Mathf.Floor(inputPosition.y));

        Vector2Int chunkPosition = new(
            (int)Mathf.Floor(position.x / 100F),
            (int)Mathf.Floor(position.y / 100F));
        if (!Chunks.ContainsKey(chunkPosition))
            throw new ArgumentOutOfRangeException(
                "Given position data doesn't exist.");
        return chunkPosition;
    }

    private static Vector2Int GetVerticeCoordinates(Vector2 inputPosition)
    {
        return new(
            (int)(inputPosition.x % 100 < 0
                ? 100 - Mathf.Abs(inputPosition.x % 100)
                : inputPosition.x % 100),
            (int)(inputPosition.y % 100 < 0
                ? 100 - Mathf.Abs(inputPosition.y % 100)
                : inputPosition.y % 100));
    }
}