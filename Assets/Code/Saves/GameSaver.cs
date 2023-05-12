using System.IO;
using System.IO.Compression;
using Input;
using UnityEngine;
using World;
using static UnityEngine.InputSystem.InputAction;

namespace Saves
{
    public class GameSaver : MonoBehaviour
    {
        #region Fields

        private string _allSavesPath;
        private string _worldPath = string.Empty;

        #endregion Fields

        #region Unity

        private void Awake()
        {
            _allSavesPath = Path.Combine(Application.persistentDataPath, "Saves");
        }

        private void OnEnable()
        {
            PlayerController.MainSave.AddListener(ActionType.Started, SaveGame);
        }

        private void OnDisable()

        {
            PlayerController.MainSave.RemoveListener(ActionType.Started, SaveGame);
        }

        #endregion Unity

        #region Private

        private void SaveGame(CallbackContext context)
        {
            Directory.CreateDirectory(_allSavesPath);
            _worldPath = Path.Combine(_allSavesPath, "A");
            Directory.CreateDirectory(_worldPath);
            SaveWorldInfo();
            SavePlayerInfo();
            SaveChunksInfo();
            File.Delete(_worldPath + ".zip");
            ZipFile.CreateFromDirectory(_worldPath, _worldPath + ".zip");
            Directory.Delete(_worldPath, true);
        }

        private void SaveWorldInfo()
        {
            string worldInfoPath = Path.Combine(_worldPath, "World.txt");
            WorldInfo worldInfo = new();
            string json = JsonUtility.ToJson(worldInfo);
            File.WriteAllText(worldInfoPath, json);
        }

        private void SavePlayerInfo()
        {
            string playerInfoPath = Path.Combine(_worldPath, "Player.txt");
            PlayerInfo playerInfo = new();
            string json = JsonUtility.ToJson(playerInfo);
            File.WriteAllText(playerInfoPath, json);
        }

        private void SaveChunksInfo()
        {
            string allChunksPath = Path.Combine(_worldPath, "Chunks");
            Directory.CreateDirectory(allChunksPath);

            foreach (Chunk chunk in World.Terrain.Chunks.Values)
            {
                string chunkPath = Path.Combine(allChunksPath, chunk.Position.ToString() + ".txt");
                string json = JsonUtility.ToJson(new ChunkInfo(chunk));
                File.WriteAllText(chunkPath, json);
            }
        }

        #endregion Private
    }
}