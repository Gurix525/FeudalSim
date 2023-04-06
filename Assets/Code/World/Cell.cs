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
        public float Steepness { get; private set; }
        public Color Color { get; private set; }

        public List<int> FloorHeights { get; } = new();
        public List<int> HorizontalWallHeights { get; } = new();
        public List<int> VerticalWallHeights { get; } = new();

        public bool HasGrass => !(
            Steepness > 0.4F
            || Color != new Color(0F, 0F, 0F, 0F)
            || FloorHeights.Contains(Height));

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

        public void SetHeight(int height)
        {
            Height = height;
        }

        public void SetSteepness(float steepness)
        {
            Steepness = steepness;
        }

        public void SetColor(Color red)
        {
            Color = red;
        }

        public override string ToString()
        {
            return $"{Position}";
        }

        #endregion Public
    }
}