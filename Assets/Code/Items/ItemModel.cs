using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace Items
{
    public class ItemModel
    {
        #region Properties

        private Mesh[] _buildingMeshes;
        private Material _material;

        public string Name { get; }
        public string Description { get; }
        public int MaxStack { get; }
        public Sprite Sprite { get; }
        public Dictionary<string, float> Stats { get; }
        public bool IsEligibleForBuilding { get; }
        public Material Material => _material ??= Materials.GetMaterial(Name) ?? Materials.DefaultMaterial;

        public Mesh[] BuildingMeshes =>
            _buildingMeshes ??=
                new Mesh[5]
                {
                    Meshes.GetMesh(Name + "Floor") ?? Meshes.GetMesh("Floor"),
                    Meshes.GetMesh(Name + "BigFloor") ?? Meshes.GetMesh("BigFloor"),
                    Meshes.GetMesh(Name + "ShortWall") ?? Meshes.GetMesh("ShortWall"),
                    Meshes.GetMesh(Name + "Wall") ?? Meshes.GetMesh("Wall"),
                    Meshes.GetMesh(Name + "BigWall") ?? Meshes.GetMesh("BigWall")
                };

        #endregion Properties

        #region Constructors

        public ItemModel(
            string name,
            int maxStack = 10,
            string description = "",
            Dictionary<string, float> stats = null,
            bool isEligibleForBuilding = false)
        {
            Name = name;
            Description = description;
            MaxStack = maxStack;
            Stats = stats;
            Sprite = Sprites.GetSprite(name);
            IsEligibleForBuilding = isEligibleForBuilding;
        }

        #endregion Constructors
    }
}