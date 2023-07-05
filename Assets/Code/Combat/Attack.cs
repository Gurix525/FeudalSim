using System;
using Controls;
using UnityEngine;
using UnityEngine.Events;
using VFX;
using WorldUI;

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
        public Action OnDamageDealt { get; set; }

        public UnityEvent<Hitbox, Vector3> DealedHit { get; } = new();

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
            DealedHit.AddListener(PlayAttackEffects);
        }

        protected void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out Hitbox hitbox);
            if (hitbox.Receiver == Sender)
                return;
            if (Target != null)
                if (hitbox.Receiver != Target)
                    return;
            DealedHit.Invoke(hitbox,
                this is IExactHitPoint
                ? other.ClosestPoint(transform.position)
                : other.transform.position + Vector3.up);
        }

        #endregion Unity

        #region Private

        private void PlayAttackEffects(Hitbox hitbox, Vector3 contact)
        {
            if (Sender == PlayerControls.Player.Instance)
                CameraShake.ShakeCamera(Damage);
            Effect.Spawn("Hit", contact);
            Effect.Spawn("BloodCloud", contact);
            Effect.Spawn("BloodSplatter", contact);
            Popup.Spawn("Damage", contact, Damage.ToString());
        }

        #endregion Private
    }
}