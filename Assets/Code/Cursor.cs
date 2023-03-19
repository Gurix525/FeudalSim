using UnityEngine;

public static class Cursor
{
    private static Vector2? _terrainCell;

    public static Vector2? TerrainCell
    {
        get => _terrainCell ?? Vector2.zero;
        set
        {
            _terrainCell = value;
            IsAboveTerrain = _terrainCell != null ? true : false;
        }
    }

    public static bool IsAboveTerrain { get; private set; } = false;
}