using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

namespace Items
{
    public class ItemModel
    {
        #region Fields

        private Mesh[] _buildingMeshes;
        private Material _material;
        private ItemAction _currentAction;
        private ItemAction[] _actions;

        #endregion Fields

        #region Properties

        public string Name { get; }
        public string Description { get; }
        public int MaxStack { get; }
        public Sprite Sprite { get; }
        public Dictionary<string, float> Stats { get; }
        public Material Material => _material ??= Materials.GetMaterial(Name) ?? Materials.DefaultMaterial;
        public ItemAction Action => _currentAction;
        public ItemAction[] Actions => _actions;

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
            ItemAction[] actions = null)
        {
            Name = name;
            Description = description;
            MaxStack = maxStack;
            Stats = stats;
            Sprite = Sprites.GetSprite(name);
            if (actions == null)
            {
                _actions = new ItemAction[] { new PutAction(), new NoAction() };
                _currentAction = _actions[0];
            }
            else
            {
                var newActions = actions.ToList();
                newActions.Insert(0, new PutAction());
                newActions.Add(new NoAction());
                _actions = newActions.ToArray();
                _currentAction = _actions[1];
            }
        }

        #endregion Constructors
    }
}