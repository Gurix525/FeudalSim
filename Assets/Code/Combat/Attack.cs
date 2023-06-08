﻿using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Attack : MonoBehaviour
    {
        #region Properties

        public Component Sender { get; set; }
        public Component Target { get; set; }
        public float Damage { get; set; }

        public UnityEvent<Hitbox> DealedHit { get; } = new();

        #endregion Properties

        #region Unity

        private void Awake()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer("Attack");
            Sender = this;
        }

        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out Hitbox hitbox);
            if (hitbox.Receiver == Sender)
                return;
            if (Target != null)
                if (hitbox.Receiver != Target)
                    return;
            DealedHit.Invoke(hitbox);
        }

        #endregion Unity
    }
}