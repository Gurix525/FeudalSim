using System;
using System.Collections;
using System.Threading.Tasks;
 
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
            // To be added
            //new TaskManager.Task(Exit());
        }

        // To be added
        //private IEnumerator Exit()
        //{
        //    LoadingScreen.Enable();
        //    yield return null;
        //    PlayerController.PauseMenu.Disable();
        //    _gameSaver.SaveGame();
        //    AsyncOperation loading = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        //    while (!loading.isDone)
        //        yield return null;
        //    PlayerController.Main.Enable();
        //    Time.timeScale = 1F;
        //    yield return null;
        //    LoadingScreen.Disable();
        //}
    }
}