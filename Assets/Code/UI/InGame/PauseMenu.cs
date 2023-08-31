using System.Collections.Generic;
using Extensions;
using Input;
using Items;
using Misc;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static UnityEngine.InputSystem.InputAction;
using Cursor = Controls.Cursor;
using UnityEngine.Events;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _returnToGameButton;

        #endregion Fields

        #region Properties

        public static UnityEvent<ItemAction, int> Closed { get; } = new();

        #endregion Properties

        #region Unity

        private void Start()
        {
            _pauseMenu.SetActive(false);
        }

        private void OnEnable()
        {
            PlayerController.MainPauseMenu.AddListener(ActionType.Started, OpenPauseMenu);
        }

        private void OnDisable()
        {
            PlayerController.MainPauseMenu.RemoveListener(ActionType.Started, OpenPauseMenu);
        }

        #endregion Unity

        #region Private

        private void OpenPauseMenu(CallbackContext context)
        {
            Time.timeScale = 0F;
            _pauseMenu.SetActive(true);
            PlayerController.Main.Disable();
            PlayerController.PauseMenu.Enable();
            PlayerController.PauseMenuPauseMenu.AddListener(ActionType.Started, ClosePauseMenu);
            _returnToGameButton.Clicked.AddListener(ClosePauseMenu);
        }

        private void ClosePauseMenu(CallbackContext context)
        {
            Time.timeScale = 1F;
            _pauseMenu.SetActive(false);
            PlayerController.PauseMenu.Disable();
            PlayerController.Main.Enable();
            PlayerController.QuickMenuQuickMenu.RemoveListener(ActionType.Started, ClosePauseMenu);
            _returnToGameButton.Clicked.RemoveListener(ClosePauseMenu);
        }

        private void ClosePauseMenu()
        {
            ClosePauseMenu(new());
        }

        #endregion Private
    }
}