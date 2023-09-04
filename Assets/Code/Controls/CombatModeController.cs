using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class CombatModeController : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerController.MainChange.AddListener(ActionType.Started, SwitchCombatMode);
        }

        private void OnDisable()
        {
            PlayerController.MainChange.RemoveListener(ActionType.Started, SwitchCombatMode);
        }

        private void SwitchCombatMode(InputAction.CallbackContext context)
        {
            Controls.Cursor.IsCombatMode ^= true;
        }
    }
}