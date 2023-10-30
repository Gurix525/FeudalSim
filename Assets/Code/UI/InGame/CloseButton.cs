using UnityEngine;

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