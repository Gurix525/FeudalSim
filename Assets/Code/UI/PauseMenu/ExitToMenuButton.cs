using System.Collections;

using Saves;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ExitToMenuButton : Button
    {
        [SerializeField] private GameSaver _gameSaver;
        [SerializeField] private PlayerInput _playerInput;

        protected override void Execute()
        {
            new TaskManager.Task(Exit());
        }

        private void OnLeftMouseButton(InputValue value)
        { }

        private IEnumerator Exit()
        {
            LoadingScreen.Enable();
            yield return null;
            _playerInput.SwitchCurrentActionMap("Main");
            InputActionMap currentActionMap = _playerInput.currentActionMap;
            currentActionMap.Disable();
            _gameSaver.SaveGame();
            AsyncOperation loading = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            while (!loading.isDone)
                yield return null;
            currentActionMap.Enable();
            Time.timeScale = 1F;
            yield return null;
            LoadingScreen.Disable();
        }
    }
}