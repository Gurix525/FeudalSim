using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace Items
{
    public class ItemModel
    {
        #region Fields

        private Mesh[] _buildingMeshes;
        private Mesh _mesh;
        private Material _material;
        private GameObject _weaponPrefab;

        #endregion Fields

        #region Properties

        public string Name { get; }
        public string Description { get; }
        public Sprite Sprite { get; }
        public Dictionary<string, string> Stats { get; }
        public Recipe Recipe { get; }
        public bool IsDiscovered { get; private set; } = false;
        public Material Material => _material ??= Materials.GetMaterial(Name) ?? Materials.DefaultMaterial;

        public GameObject WeaponPrefab =>
            _weaponPrefab ??= Resources.Load<GameObject>("Prefabs/Weapons/" + Name);

        public Mesh[] BuildingMeshes =>
            _buildingMeshes ??=
                new Mesh[5]
                {
                    Resources.Load<Mesh>("Meshes/Buildings/" + Name + "/Floor")
                        ?? Resources.Load<Mesh>("Meshes/Buildings/Floor"),
                    Resources.Load<Mesh>("Meshes/Buildings/" + Name + "/BigFloor")
                        ?? Resources.Load<Mesh>("Meshes/Buildings/BigFloor"),
                    Resources.Load<Mesh>("Meshes/Buildings/" + Name + "/ShortWall")
                        ?? Resources.Load<Mesh>("Meshes/Buildings/ShortWall"),
                    Resources.Load<Mesh>("Meshes/Buildings/" + Name + "/Wall")
                        ?? Resources.Load<Mesh>("Meshes/Buildings/Wall"),
                    Resources.Load<Mesh>("Meshes/Buildings/" + Name + "/BigWall")
                        ?? Resources.Load<Mesh>("Meshes/Buildings/BigWall")
                };

        public Mesh Mesh =>
            _mesh ??= Resources.Load<Mesh>("Meshes/Items/" + Name);

        #endregion Properties

        #region Constructors

        public ItemModel(
            string name,
            string description = "",
            Sprite sprite = null,
            Dictionary<string, string> stats = null,
            Recipe recipe = null)
        {
            Name = name;
            Description = description;
            Stats = stats ?? new();
            Sprite = sprite;
            Recipe = recipe;
        }

        public ItemModel(ItemScriptableObject item)
        {
            Name = item.name;
            Description = item.Description;
            Stats = new();
            Sprite = item.Sprite;
            Recipe = item.Recipe;
        }

        #endregion Constructors

        #region Public

        public void MarkDiscovered()
        {
            IsDiscovered = true;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion Public
    }
}