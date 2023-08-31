using System;
using System.Threading.Tasks;
using Input;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ExitToMenuButton : Button
    {
        [SerializeField] private GameSaver _gameSaver;

        protected override void Execute()
        {
            PlayerController.PauseMenu.Disable();
            _gameSaver.SaveGame();
            Time.timeScale = 1F;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            PlayerController.Main.Enable();
        }
    }
}