using System.Collections.Generic;
using System.Linq;
using Misc;
using UI;
using UnityEngine;
using Cursor = Controls.PlayerCursor;

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
            Dictionary<string, string> stats = null)
        {
            Name = name;
            Description = description;
            Stats = stats ?? new();
            Sprite = Resources.Load<Sprite>("Sprites/Items/" + name);
        }

        #endregion Constructors
    }
}