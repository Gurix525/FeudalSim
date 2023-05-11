using System;
using System.IO;
using System.Linq;
using Input;
using Saves;
using UnityEngine;
using World;
using static UnityEngine.InputSystem.InputAction;

public class GameSaver : MonoBehaviour
{
    private string _allSavesPath;
    private string _worldPath = string.Empty;

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

    #region Private

    private void SaveGame(CallbackContext context)
    {
        Directory.CreateDirectory(_allSavesPath);
        _worldPath = Path.Combine(_allSavesPath, "A");
        Directory.CreateDirectory(_worldPath);
        SaveWorldInfo();
        SavePlayerInfo();
        SaveChunksInfo();
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
        //Chunk chunk = World.Terrain.Chunks.Values.Last();
        //string chunkPath = Path.Combine(allChunksPath, chunk.Position.ToString() + ".txt");
        //string json = JsonUtility.ToJson(new ChunkInfo(chunk));
        //File.WriteAllText(chunkPath, json);
    }

    #endregion Private
}