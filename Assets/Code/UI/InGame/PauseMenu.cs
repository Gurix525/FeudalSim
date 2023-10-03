using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _returnToGameButton;

        #endregion Fields

        #region Unity

        private void Start()
        {
            _pauseMenu.SetActive(false);
        }

        // To be added
        //private void OnEnable()
        //{
        //    PlayerController.MainPauseMenu.AddListener(ActionType.Started, OpenPauseMenu);
        //}

        //private void OnDisable()
        //{
        //    PlayerController.MainPauseMenu.RemoveListener(ActionType.Started, OpenPauseMenu);
        //}

        #endregion Unity

        #region Private

        // To be added
        //private void OpenPauseMenu(CallbackContext context)
        //{
        //    Time.timeScale = 0F;
        //    _pauseMenu.SetActive(true);
        //    PlayerController.Main.Disable();
        //    PlayerController.PauseMenu.Enable();
        //    PlayerController.PauseMenuPauseMenu.AddListener(ActionType.Started, ClosePauseMenu);
        //    _returnToGameButton.Clicked.AddListener(ClosePauseMenu);
        //}

        // To be added
        //private void ClosePauseMenu(CallbackContext context)
        //{
        //    Time.timeScale = 1F;
        //    _pauseMenu.SetActive(false);
        //    PlayerController.PauseMenu.Disable();
        //    PlayerController.Main.Enable();
        //    PlayerController.QuickMenuQuickMenu.RemoveListener(ActionType.Started, ClosePauseMenu);
        //    _returnToGameButton.Clicked.RemoveListener(ClosePauseMenu);
        //}

        //private void ClosePauseMenu()
        //{
        //    ClosePauseMenu(new());
        //}

        #endregion Private
    }
}