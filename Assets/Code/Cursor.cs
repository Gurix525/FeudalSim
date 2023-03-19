using UnityEngine;

public static class Cursor
{
    private static Cell _cell;

    public static Cell Cell
    {
        get => _cell ?? null;
        set
        {
            _cell = value;
            IsAboveTerrain = _cell != null ? true : false;
        }
    }

    public static bool IsAboveTerrain { get; private set; } = false;
}