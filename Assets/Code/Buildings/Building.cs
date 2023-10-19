using Items;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField][TextArea(10, 20)] private string _components;

        public Container ComponentsContainer { get; } = new(10);
    }
}