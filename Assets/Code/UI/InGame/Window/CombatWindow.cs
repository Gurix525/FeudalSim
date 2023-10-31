using Controls;
using PlayerControls;
using UnityEngine;

namespace UI
{
    public class CombatWindow : Window
    {
        [SerializeField] private RightHandItemHook _rightHandItemHook;

        public void SwitchActive()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        private void OnEnable()
        {
            if (CombatCursor.Current != null)
                CombatCursor.Current.gameObject.SetActive(true);
            if (_rightHandItemHook != null)
                _rightHandItemHook.SetItemActive("Sword", true);
        }

        private void OnDisable()
        {
            if (CombatCursor.Current != null)
                CombatCursor.Current.gameObject.SetActive(false);
            if (_rightHandItemHook != null)
                _rightHandItemHook.SetItemActive("Sword", false);
        }
    }
}