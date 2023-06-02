using System;
using System.Linq;
using Buildings;
using Items;
using UnityEngine;
using World;

namespace Saves
{
    [Serializable]
    public class ChunkInfo
    {
        #region Fields

        public Vector2Int Position;
        public bool IsNatureSpawned;
        public int[] Heights;
        public float[] Steepnesses;
        public Color[] Colors;
        public string[] FloorHeights;
        public string[] HorizontalWallHeights;
        public string[] VerticalWallHeights;
        public BuildingInfo[] Buildings;
        public ItemHandlerInfo[] ItemHandlers;
        public TreeInfo[] Trees;
        public BoulderInfo[] Boulders;

        #endregion Fields

        #region Constructors

        public ChunkInfo()
        {
        }

        public ChunkInfo(Chunk chunk)
        {
            Initialize(chunk);
        }

        #endregion Constructors

        #region Public

        public void Initialize(Chunk chunk)
        {
            Position = chunk.Position;
            IsNatureSpawned = chunk.IsNatureSpawned;
            Heights = new int[2500];
            Steepnesses = new float[2500];
            Colors = new Color[2500];
            FloorHeights = new string[2500];
            HorizontalWallHeights = new string[2500];
            VerticalWallHeights = new string[2500];
            SetTerrainInfo(chunk);

            ChunkRenderer chunkRenderer = TerrainRenderer.GetChunkRenderer(chunk);
            if (chunkRenderer == null)
                return;
            SetBuildingsInfo(chunkRenderer);
            SetItemHandlersInfo(chunkRenderer);
            SetTreesInfo(chunkRenderer);
            SetBouldersInfo(chunkRenderer);
        }

        #endregion Public

        #region Private

        private void SetTerrainInfo(Chunk chunk)
        {
            Cell[] cells = chunk.Cells.Values.ToArray();
            for (int i = 0; i < 2500; i++)
            {
                Cell cell = cells[i];
                Heights[i] = cell.Height;
                Steepnesses[i] = cell.Steepness;
                Colors[i] = cell.Color;
                FloorHeights[i] = string.Join(
                    ',',
                    cell.FloorHeights
                    .Select(x => x.ToString()));
                HorizontalWallHeights[i] = string.Join(
                    ',',
                    cell.HorizontalWallHeights
                    .Select(x => x.ToString()));
                VerticalWallHeights[i] = string.Join(
                    ',',
                    cell.VerticalWallHeights
                    .Select(x => x.ToString()));
            }
        }

        private void SetBuildingsInfo(ChunkRenderer chunkRenderer)
        {
            Buildings = chunkRenderer.Buildings
                            .GetComponentsInChildren<Building>()
                            .Select(building => new BuildingInfo(building))
                            .ToArray();
        }

        private void SetItemHandlersInfo(ChunkRenderer chunkRenderer)
        {
            ItemHandlers = chunkRenderer.ItemHandlers
                            .GetComponentsInChildren<ItemHandler>()
                            .Select(handler => new ItemHandlerInfo(handler))
                            .ToArray();
        }

        private void SetTreesInfo(ChunkRenderer chunkRenderer)
        {
            Trees = chunkRenderer.Trees
                            .GetComponentsInChildren<Nature.Tree>()
                            .Select(tree => new TreeInfo(tree))
                            .ToArray();
        }

        private void SetBouldersInfo(ChunkRenderer chunkRenderer)
        {
            Boulders = chunkRenderer.Boulders
                            .GetComponentsInChildren<Nature.Boulder>()
                            .Select(boulder => new BoulderInfo(boulder))
                            .ToArray();
        }

        #endregion Private
    }
}