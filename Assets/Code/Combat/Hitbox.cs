using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {
        private Collider _collider;

        public UnityEvent<Attack> GotHit { get; } = new();

        public Component Receiver { get; set; }

        private void Awake()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Hitbox");
        }

        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out Attack attack);
            if (attack.Sender == Receiver)
                return;
            if (attack.Target != null)
                if (attack.Target != Receiver)
                    return;
            GotHit.Invoke(attack);
        }
    }
}