using UnityEngine;

namespace UI
{
    public class CombatCanvas : MonoBehaviour
    {
        [SerializeField] private Canvases _canvases;
        [SerializeField] private CombatWindow _combatWindow;

        private void OnEnable()
        {
            _canvases.CommandPassed += _canvases_CommandPassed;
        }

        private void OnDisable()
        {
            _canvases.CommandPassed -= _canvases_CommandPassed;
        }

        private void _canvases_CommandPassed(object sender, string e)
        {
            if (e == "Combat")
                _combatWindow.SwitchActive();
            else
                _combatWindow.gameObject.SetActive(false);
        }
    }
}