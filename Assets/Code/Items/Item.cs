using System;
using System.Collections.Generic;
using System.Linq;
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

        private static Dictionary<string, ItemModel> _itemModels = new();
        //private static Dictionary<string, ItemModel> _itemModels = new()
        //{
        //    { "Stone", new("Stone")},
        //    { "Wood", new("Wood")},
        //    { "Plank", new("Plank") },
        //    { "Sword", new("Sword") },
        //    { "Bow", new("Bow") },
        //    { "Axe", new("Axe") },
        //    { "Pickaxe", new("Pickaxe") },
        //    { "Shovel", new("Shovel") },
        //    { "Workbench", new("Workbench")},
        //    { "Helmet", new ("Helmet", stats: new(){ { "ArmorType", "Head" }, { "InventorySlots", "3"} }) }
        //};

        #endregion Fields

        #region Properties

        public int Count { get; set; }
        public string Name => _model.Name;
        public string Description => _model.Description;
        public ItemModel Model => _model;
        public Sprite Sprite => _model.Sprite;
        public Recipe Recipe => _model.Recipe;
        public Material Material => _model.Material;
        public Mesh[] BuildingMeshes => _model.BuildingMeshes;
        public Mesh Mesh => _model.Mesh;
        public bool HasSpecificStats => _stats != null;
        public GameObject WeaponPrefab => _model.WeaponPrefab;

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

        public static IEnumerable<ItemModel> ItemModels => _itemModels.Values;

        #endregion Properties

        #region Constructors

        private Item(ItemModel model, int count = 1, Dictionary<string, string> stats = null)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("Liczba przedmiotów nie może" +
                    "być mniejsza niż 1.");
            _model = model;
            Count = count;
            _stats = stats;
        }

        #endregion Constructors

        #region Public

        public void Drop(Vector3 dropPosition, bool hasToScatter = true)
        {
            Drop(dropPosition, Quaternion.identity, hasToScatter);
        }

        public void Drop(Vector3 dropPosition, Quaternion rotation, bool hasToScatter = true)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Items/" + Name);
            if (prefab == null)
                return;
            ItemHandler itemHandler = GameObject
                .Instantiate(
                prefab,
                TerrainRenderer.GetChunkRenderer(
                    Terrain.GetChunkCoordinates(
                        new Vector2(dropPosition.x, dropPosition.z))).ItemHandlers)
                .GetComponent<ItemHandler>();
            itemHandler.Container.InsertAt(0, this);
            itemHandler.transform.SetPositionAndRotation(
                dropPosition + (hasToScatter ? GetRandomScatterOffset() : Vector3.zero),
                rotation);
            ScatterItem(itemHandler);
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
            if (name == null)
                throw new ArgumentNullException("Name cannot be null");
            if (!_itemModels.TryGetValue(name, out ItemModel itemModel))
                return null;
            return new(itemModel, count, stats);
        }

        public static IEnumerable<Item> GetFromRecipe(Recipe recipe)
        {
            List<Item> items = new();
            foreach (var item in recipe.Items)
            {
                items.Add(Create(item.Item.name, item.Count));
            }
            return items;
        }

        public static ItemModel GetModel(string name)
        {
            _itemModels.TryGetValue(name, out ItemModel itemModel);
            return itemModel;
        }


        #endregion Public

        #region Private

        private void ScatterItem(ItemHandler itemHandler)
        {
            Vector2 position = new(itemHandler.transform.position.x, itemHandler.transform.position.z);
            Transform parent = TerrainRenderer.GetChunkRenderer(
                Terrain.GetChunkCoordinates(
                    position)).ItemHandlers;
            foreach (Transform child in parent)
                _ = child.GetComponent<ItemHandler>().ScatterItem();
        }

        private Vector3 GetRandomScatterOffset()
        {
            Random random = new Random();
            float x = ((float)random.NextDouble()).Remap(0F, 1F, -0.3F, 0.3F);
            float y = ((float)random.NextDouble()).Remap(0F, 1F, 0.1F, 0.2F);
            float z = ((float)random.NextDouble()).Remap(0F, 1F, -0.3F, 0.3F);
            return new Vector3(x, y, z);
        }

        public static void LoadResources()
        {
            _itemModels = Resources
                .LoadAll<ItemScriptableObject>("ScriptableObjects/Items")
                .ToDictionary(item => item.name, item => new ItemModel(item));
        }

        #endregion Private
    }
}