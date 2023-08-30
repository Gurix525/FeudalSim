using System;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class Cell
    {
        #region Properties

        public Vector2Int Position { get; private set; }
        public int Height { get; private set; }
        public float Steepness { get; set; }
        public Color Color { get; set; }

        public List<int> FloorHeights { get; set; } = new();
        public List<int> HorizontalWallHeights { get; set; } = new();
        public List<int> VerticalWallHeights { get; set; } = new();

        public bool HasGrass => !(
            Steepness > 0.4F
            || Height < 1F
            || Color != GrassVerticeColor
            || FloorHeights.Contains(Height));

        public static Color GrassVerticeColor { get; } = new Color(0.75F, 1F, 0.4F, 1F).linear;
        public static Color SandVerticeColor { get; } = new Color(1F, 0.95F, 0.85F).linear;

        #endregion Properties

        #region Constructors

        public Cell(
            Vector2Int position,
            int height,
            float steepness = 0F,
            Color color = new())
        {
            Position = position;
            Height = height;
            Steepness = steepness;
            Color = color;
        }

        #endregion Constructors

        #region Public

        public void SetBuildingMark(BuildingMarkType markType, int height, bool isAdding)
        {
            List<int> heights = markType switch
            {
                BuildingMarkType.Floor => FloorHeights,
                BuildingMarkType.HorizontalWall => HorizontalWallHeights,
                _ => VerticalWallHeights,
            };
            if (isAdding)
                heights.Add(height);
            else
                heights.Remove(height);
        }

        public bool IsBuildingPossible(BuildingMarkType markType, int height)
        {
            List<int> heights = markType switch
            {
                BuildingMarkType.Floor => FloorHeights,
                BuildingMarkType.HorizontalWall => HorizontalWallHeights,
                _ => VerticalWallHeights,
            };
            if (height < Height)
                return false;
            return !heights.Contains(height);
        }

        public void RecalculateSteepness()
        {
            float min = Mathf.Min(
                Height,
                Terrain.GetHeight(new Vector2Int(Position.x + 1, Position.y)),
                Terrain.GetHeight(new Vector2Int(Position.x, Position.y + 1)),
                Terrain.GetHeight(new Vector2Int(Position.x + 1, Position.y + 1)));
            float max = Mathf.Max(
                Height,
                Terrain.GetHeight(new Vector2Int(Position.x + 1, Position.y)),
                Terrain.GetHeight(new Vector2Int(Position.x, Position.y + 1)),
                Terrain.GetHeight(new Vector2Int(Position.x + 1, Position.y + 1)));
            Steepness = max - min;
        }

        public void ModifyHeight(int deltaHeight)
        {
            Height += deltaHeight;
        }

        public override string ToString()
        {
            return $"{Position}";
        }

        #endregion Public
    }
}