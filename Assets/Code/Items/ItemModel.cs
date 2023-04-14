using System.Collections.Generic;
using System.Linq;
using Misc;
using UI;
using UnityEngine;
using Cursor = Controls.Cursor;

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

        public ItemAction[] Actions
        {
            get
            {
                List<ItemAction> actions = new();
                foreach (var action in _actions)
                {
                    if (action is BuildAction)
                        for (int i = 0; i < 5; i++)
                            actions.Add(new BuildAction());
                    else if (action is ShovelAction)
                        for (int i = 0; i < 4; i++)
                            actions.Add(new ShovelAction());
                    else
                        actions.Add(action);
                }
                return actions.ToArray();
            }
        }

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
            Sprite = Resources.Load<Sprite>("Sprites/Items/" + name);
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
            QuickMenu.Closed.AddListener(ChangeAction);
        }

        #endregion Constructors

        #region Private

        private void ChangeAction(ItemAction action, int slotNumber)
        {
            if (Cursor.Item == null)
                return;
            if (Cursor.Item.Model != this)
                return;
            _currentAction = action;
            (_currentAction as BuildAction)?.ReloadCursorMeshHighlight();
        }

        #endregion Private
    }
}