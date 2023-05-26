using UnityEngine;

namespace AI
{
    public class AIBehaviour : MonoBehaviour
    {
        public Animal Animal { get; set; }
        public Agent Agent { get; set; }

        public Component Focus => Animal.Focus;

        protected void Awake()
        {
            enabled = false;
        }
    }
}