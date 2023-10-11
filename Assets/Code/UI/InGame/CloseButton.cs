 
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public class CloseButton : Button
    {
        [SerializeField] private GameObject _windowToClose;

        protected override void Execute()
        {
            _windowToClose.SetActive(false);
        }
    }
}