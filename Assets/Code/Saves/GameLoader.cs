using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Buildings;
using Controls;
using Items;
using Misc;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;
using Terrain = World.Terrain;

namespace Saves
{
    public class GameLoader
    {
        #region Fields

        private string _worldName;
        private string _savePath;
        private GameObject[] _buildingPrefabs;

        #endregion Fields

        #region Properties

        private GameObject[] BuildingPrefabs => _buildingPrefabs ??= new GameObject[5]
        {
            Resources.Load<GameObject>("Prefabs/Buildings/Floor"),
            Resources.Load<GameObject>("Prefabs/Buildings/BigFloor"),
            Resources.Load<GameObject>("Prefabs/Buildings/ShortWall"),
            Resources.Load<GameObject>("Prefabs/Buildings/Wall"),
            Resources.Load<GameObject>("Prefabs/Buildings/BigWall")
        };

        #endregion Properties

        #region Constructors

        public GameLoader(string worldName)
        {
            _worldName = worldName;
            _savePath = Path.Combine(
                Application.persistentDataPath, "Saves", _worldName);
        }

        #endregion Constructors

        #region Public

        public async Task LoadGame()
        {
            try
            {
                ZipFile.ExtractToDirectory(_savePath + ".zip", _savePath);
                LoadWorld();
                var playerinfo = LoadPlayer();
                ChunkInfo[] chunkInfos = LoadChunks();
                TerrainRenderer.GenerateWorld(Terrain.GetChunkCoordinates(
                    new Vector2(playerinfo.Position.x, playerinfo.Position.z)));
                LoadChunkRenderers(chunkInfos);
                GrassInstancer.MarkToReload();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + e.StackTrace);
                AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
                while (!sceneLoading.isDone)
                    await Task.Yield();
            }
            finally
            {
                Directory.Delete(_savePath, true);
                LoadingScreen.Disable();
            }
        }

        #endregion Public

        #region Private

        private void LoadWorld()
        {
            Terrain.Reset();
            WorldInfo worldInfo = JsonUtility.FromJson<WorldInfo>(
                File.ReadAllText(Path.Combine(_savePath, "World.txt")));
            NoiseSampler.SetSeed(worldInfo.Seed);
            GameManager.WorldName = worldInfo.Name;
            GameManager.WorldCreationTime = worldInfo.CreationTime;
            GameManager.FullTimeInWorld = worldInfo.FullTimeInWorld;
            GameManager.LastPlayedTime = worldInfo.LastPlayedTime;
        }

        private PlayerInfo LoadPlayer()
        {
            PlayerInfo playerInfo = JsonUtility.FromJson<PlayerInfo>(
                File.ReadAllText(Path.Combine(_savePath, "Player.txt")));
            var player = References.GetReference("Player");
            player.SetActive(false);
            player.transform.position = playerInfo.Position;
            player.SetActive(true);
            Equipment.SetInventoryContainer((Container)playerInfo.InventoryContainer);
            Equipment.SetArmorContainer((Container)playerInfo.ArmorContainer);
            return playerInfo;
        }

        private ChunkInfo[] LoadChunks()
        {
            string[] fileNames = Directory.GetFiles(Path.Combine(_savePath, "Chunks"));
            List<ChunkInfo> chunkInfos = new();
            for (int i = 0; i < fileNames.Length; i++)
            {
                ChunkInfo chunkInfo = JsonUtility.FromJson<ChunkInfo>(
                    File.ReadAllText(fileNames[i]));
                Chunk chunk = Terrain.Chunks[chunkInfo.Position] = new(chunkInfo);
                chunk.IsNatureSpawned = chunkInfo.IsNatureSpawned;
                chunkInfos.Add(chunkInfo);
            }
            return chunkInfos.ToArray();
        }

        private void LoadChunkRenderers(ChunkInfo[] chunkInfos)
        {
            foreach (var chunkInfo in chunkInfos)
            {
                ChunkRenderer chunkRenderer = TerrainRenderer.GetChunkRenderer(
                    Terrain.Chunks[chunkInfo.Position]);
                if (chunkRenderer == null)
                    continue;
                LoadBuildings(chunkInfo, chunkRenderer);
                LoadItemHandlers(chunkInfo, chunkRenderer);
                LoadTrees(chunkInfo, chunkRenderer);
                LoadBoulders(chunkInfo, chunkRenderer);
            }
        }

        private void LoadBuildings(ChunkInfo chunkInfo, ChunkRenderer chunkRenderer)
        {
            foreach (var buildingInfo in chunkInfo.Buildings)
            {
                GameObject prefab = BuildingPrefabs[(int)buildingInfo.BuildingMode];
                GameObject building = GameObject.Instantiate(
                    prefab, chunkRenderer.Buildings);
                building.transform.SetPositionAndRotation(
                    buildingInfo.Position, buildingInfo.Rotation);
                building.GetComponent<Building>().
                Initialize((Item)buildingInfo.BackingItem, buildingInfo.BuildingMode);
            }
        }

        private void LoadItemHandlers(ChunkInfo chunkInfo, ChunkRenderer chunkRenderer)
        {
            foreach (var itemHandlerInfo in chunkInfo.ItemHandlers)
            {
                GameObject prefab = Resources.Load<GameObject>(
                    "Prefabs/Items/" + itemHandlerInfo.Item.Name);
                if (prefab == null)
                    continue;
                ItemHandler itemHandler = GameObject
                    .Instantiate(prefab, chunkRenderer.ItemHandlers)
                    .GetComponent<ItemHandler>();
                itemHandler.Container.InsertAt(0, itemHandlerInfo.Item);
                itemHandler.transform.SetPositionAndRotation(
                    itemHandlerInfo.Position,
                    itemHandlerInfo.Rotation);
            }
        }

        private void LoadTrees(ChunkInfo chunkInfo, ChunkRenderer chunkRenderer)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Nature/Tree");
            foreach (var treeInfo in chunkInfo.Trees)
            {
                GameObject tree = GameObject.Instantiate(prefab, chunkRenderer.Trees);
                tree.transform.SetPositionAndRotation(
                    treeInfo.Position, treeInfo.Rotation);
                tree.transform.localScale = treeInfo.Scale;
            }
        }

        private void LoadBoulders(ChunkInfo chunkInfo, ChunkRenderer chunkRenderer)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Nature/Boulder");
            foreach (var boulderInfo in chunkInfo.Boulders)
            {
                GameObject boulder = GameObject.Instantiate(prefab, chunkRenderer.Boulders);
                boulder.transform.SetPositionAndRotation(
                    boulderInfo.Position, boulderInfo.Rotation);
                boulder.transform.localScale = boulderInfo.Scale;
            }
        }

        #endregion Private
    }
}