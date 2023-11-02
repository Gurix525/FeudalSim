using Controls;
using UI;
using UnityEngine;

namespace Dialogues
{
    public class Character : MonoBehaviour, IMouseHandler
    {
        public virtual void OnLeftMouseButton(Vector2 position)
        {
            DialogueWindow.Current.ShowDialogue(Dialogue.Get(1));
        }
    }
}