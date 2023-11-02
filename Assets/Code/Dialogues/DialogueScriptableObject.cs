using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogues/Dialogue")]
    public class DialogueScriptableObject : ScriptableObject
    {
        [field: SerializeField] public Verse[] Verses { get; set; }
    }
}