using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {
        private Collider _collider;

        public UnityEvent<Attack> GotHit { get; } = new();

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Hitbox");
        }

        private void OnTriggerEnter(Collider other)
        {
            GotHit.Invoke(other.GetComponent<Attack>());
        }
    }
}