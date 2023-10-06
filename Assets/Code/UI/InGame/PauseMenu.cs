using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _returnToGameButton;
        [SerializeField] private PlayerInput _playerInput;

        #endregion Fields

        #region Input

        private void OnEscape()
        {
            if (_pauseMenu.activeInHierarchy)
                ClosePauseMenu();
            else
                OpenPauseMenu();
        }

        #endregion

        #region Unity

        private void Start()
        {
            _pauseMenu.SetActive(false);
            _returnToGameButton.Clicked += ReturnToGameButton_Clicked;
        }

        #endregion Unity

        #region Private

        private void OpenPauseMenu()
        {
            Time.timeScale = 0F;
            _pauseMenu.SetActive(true);
            _playerInput.SwitchCurrentActionMap("PauseMenu");
            //_returnToGameButton.Clicked.AddListener(ClosePauseMenu);
        }

        private void ClosePauseMenu()
        {
            Time.timeScale = 1F;
            _pauseMenu.SetActive(false);
            _playerInput.SwitchCurrentActionMap("Main");
            //_returnToGameButton.Clicked.RemoveListener(ClosePauseMenu);
        }

        private void ReturnToGameButton_Clicked(object sender, System.EventArgs e)
        {
            ClosePauseMenu();
        }

        #endregion Private
    }
}