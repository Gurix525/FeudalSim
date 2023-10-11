using System.IO;
using System.IO.Compression;

using UnityEngine;
using UnityEngine.InputSystem;
using World;

namespace Saves
{
    public class GameSaver : MonoBehaviour
    {
        #region Fields

        private string _allSavesPath;
        private string _worldPath = string.Empty;

        #endregion Fields

        #region Public

        public void SaveGame()
        {
            Directory.CreateDirectory(_allSavesPath);
            _worldPath = Path.Combine(_allSavesPath, Controls.GameManager.WorldName);
            Directory.CreateDirectory(_worldPath);
            SaveWorldInfo();
            SavePlayerInfo();
            SaveChunksInfo();
            File.Delete(_worldPath + ".zip");
            ZipFile.CreateFromDirectory(_worldPath, _worldPath + ".zip");
            Directory.Delete(_worldPath, true);
        }

        #endregion Public

        #region Input

        private void OnSave(InputValue value)
        {
            SaveGame();
        }

        #endregion Input

        #region Unity

        private void Awake()
        {
            _allSavesPath = Path.Combine(Application.persistentDataPath, "Saves");
        }

        #endregion Unity

        #region Private

        private void SaveWorldInfo()
        {
            string worldInfoPath = Path.Combine(_worldPath, "World.txt");
            WorldInfo worldInfo = new(true);
            string json = JsonUtility.ToJson(worldInfo);
            File.WriteAllText(worldInfoPath, json);
        }

        private void SavePlayerInfo()
        {
            string playerInfoPath = Path.Combine(_worldPath, "Player.txt");
            PlayerInfo playerInfo = new(true);
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