using System;
using System.IO;
using Input;
using UnityEngine;
using World;
using static UnityEngine.InputSystem.InputAction;

public class GameSaver : MonoBehaviour
{
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
        string allSavesPath = Path.Combine(Application.persistentDataPath, "Saves");
        Directory.CreateDirectory(allSavesPath);
        string savePath = Path.Combine(allSavesPath, NoiseSampler.Seed.ToString());
        Directory.CreateDirectory(savePath);
    }

    #endregion Private
}