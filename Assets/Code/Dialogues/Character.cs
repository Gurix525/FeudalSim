using Controls;
using UI;
using UnityEngine;

namespace Dialogues
{
    public class Character : MonoBehaviour, IMouseHandler
    {
        private int _dialogueNumber = 1;

        public virtual void OnLeftMouseButton(Vector2 position)
        {
            DialogueWindow.Current.ShowDialogue(Dialogue.Get(_dialogueNumber++));
            _dialogueNumber = Mathf.Clamp(_dialogueNumber, 1, 3);
        }
    }
}