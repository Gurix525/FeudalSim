using System;
using System.IO;
using Input;
using UnityEngine;
using World;
using static UnityEngine.InputSystem.InputAction;

public class GameSaver : MonoBehaviour
{
    private string _allSavesPath;
    private string _savePath = string.Empty;

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
        _savePath = Path.Combine(_allSavesPath, "A");
        Directory.CreateDirectory(_savePath);
        SaveWorldInfo();
        SavePlayerInfo();
    }

    private void SaveWorldInfo()
    {
        string worldInfoPath = Path.Combine(_savePath, "World.txt");
        string dataToWrite = "A";
        dataToWrite += $"\n{NoiseSampler.Seed}";
        File.WriteAllText(worldInfoPath, dataToWrite);
    }

    private void SavePlayerInfo()
    {
        string playerInfoPath = Path.Combine(_savePath, "Player.txt");
        Saves.PlayerInfo playerInfo = new();
        string json = JsonUtility.ToJson(playerInfo);
        File.WriteAllText(playerInfoPath, json);
    }

    #endregion Private
}