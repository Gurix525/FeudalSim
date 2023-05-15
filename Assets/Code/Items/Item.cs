using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using World;
using Random = System.Random;
using Terrain = World.Terrain;

namespace Items
{
    public class Item
    {
        #region Fields

        private ItemModel _model;
        private Dictionary<string, string> _stats;

        private static Dictionary<string, ItemModel> _itemModels = new()
        {
            { "Stone", new("Stone", actions: new ItemAction[] {new BuildAction()})},
            { "Wood", new("Wood", 20, actions: new ItemAction[] {new BuildAction()}) },
            { "Plank", new("Plank", 20, actions: new ItemAction[] {new BuildAction()}) },
            { "Sword", new("Sword", 1, stats: new(){ { "ArmorType", "Head" }, { "InventorySlots", "3"} }) },
            { "Axe", new("Axe", 1, actions: new ItemAction[] {new AxeAction(),  new DestroyAction()}) },
            { "Pickaxe", new("Pickaxe", 1, actions: new ItemAction[] {new PickaxeAction() }) },
            { "Shovel", new("Shovel", 1, actions: new ItemAction[] {new ShovelAction()}) },
            { "Workbench", new("Workbench", 1)}
        };

        #endregion Fields

        #region Properties

        public int Count { get; set; }
        public string Name => _model.Name;
        public string Description => _model.Description;
        public ItemModel Model => _model;
        public int MaxStack => _model.MaxStack;
        public Sprite Sprite => _model.Sprite;
        public Material Material => _model.Material;
        public Mesh[] BuildingMeshes => _model.BuildingMeshes;
        public Mesh Mesh => _model.Mesh;
        public ItemAction Action => _model.Action;
        public ItemAction[] Actions => _model.Actions;
        public bool HasSpecificStats => _stats != null;

        public Dictionary<string, string> Stats
        {
            get
            {
                return _stats ?? _model.Stats;
            }
            set
            {
                _stats = value;
            }
        }

        #endregion Properties

        #region Constructors

        private Item(ItemModel model, int count = 1, Dictionary<string, string> stats = null)
        {
            _model = model;
            count = count > model.MaxStack ? model.MaxStack : count;
            Count = count == 0 ? model.MaxStack : count;
            _stats = stats;
        }

        #endregion Constructors

        #region Public

        public void Drop(Vector3 dropPosition)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Items/" + Name);
            if (prefab == null)
                return;
            ItemHandler itemHandler = GameObject
                .Instantiate(prefab, TerrainRenderer.GetChunkRenderer(
                        Terrain.GetChunkCoordinates(
                            dropPosition)).ItemHandlers)
                .GetComponent<ItemHandler>();
            ScatterItem(itemHandler);
            itemHandler.Container.InsertAt(0, this);
            itemHandler.transform.SetPositionAndRotation(
                dropPosition + GetRandomScatterOffset(),
                Quaternion.identity);
        }

        public Item Clone(int count = 0)
        {
            return new(_model, count == 0 ? Count : count, _stats);
        }

        public override string ToString()
        {
            return $"{Name}: {Count}";
        }

        public static Item Create(
            string name,
            int count = 1,
            Dictionary<string, string> stats = null)
        {
            if (!_itemModels.TryGetValue(name, out ItemModel itemModel))
                return null;
            return new(itemModel, count, stats);
        }

        #endregion Public

        #region Private

        private void ScatterItem(ItemHandler itemHandler)
        {
            _ = itemHandler.ScatterItem();
        }

        private Vector3 GetRandomScatterOffset()
        {
            Random random = new Random();
            float x = ((float)random.NextDouble()).Remap(0F, 1F, -0.3F, 0.3F);
            float y = ((float)random.NextDouble()).Remap(0F, 1F, 0.1F, 0.2F);
            float z = ((float)random.NextDouble()).Remap(0F, 1F, -0.3F, 0.3F);
            return new Vector3(x, y, z);
        }

        #endregion Private
    }
}