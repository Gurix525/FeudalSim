using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class SkillsCanvas : MonoBehaviour
    {
        [SerializeField] private SkillsWindow _window;

        private void OnSkillsList(InputValue value)
        {
            _window.OpenClose();
        }
    }
}