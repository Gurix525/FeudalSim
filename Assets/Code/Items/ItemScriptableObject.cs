using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; set; }
        [field: SerializeField][field: TextArea(10, 10)] public string Description { get; set; }
        [field: SerializeField] public Recipe Recipe { get; set; }
    }
}