using System;
using Controls;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Attack : MonoBehaviour
    {
        #region Fields

        private static int _nextInt;
        private Collider _collider;

        #endregion Fields

        #region Properties

        public int ID { get; private set; }

        public Component Sender { get; set; }
        public Component Target { get; set; }
        public float Damage { get; set; }

        public UnityEvent<Hitbox> DealedHit { get; } = new();

        public Collider Collider => _collider ??= GetComponent<Collider>();

        #endregion Properties

        #region Public

        public void SetNextID()
        {
            ID = _nextInt++;
        }

        public void ForceCollision(Collider other)
        {
            OnTriggerEnter(other);
        }

        #endregion Public

        #region Unity

        protected void Awake()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer("Attack");
            Sender ??= this;
            DealedHit.AddListener(ShakeCamera);
        }

        protected void OnTriggerEnter(Collider other)
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

        #region Private

        private void ShakeCamera(Hitbox hitbox)
        {
            if (Sender == PlayerControls.Player.Instance)
                CameraShake.ShakeCamera(Damage);
        }

        #endregion Private
    }
}