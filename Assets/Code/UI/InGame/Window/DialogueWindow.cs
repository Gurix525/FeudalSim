using UnityEngine;

namespace UI
{
    public class DialogueWindow : Window
    {
        public static DialogueWindow Current { get; private set; }

        private void Awake()
        {
            Current = this;
            gameObject.SetActive(false);
        }
    }
}