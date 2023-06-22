using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Attack : MonoBehaviour
    {
        #region Fields

        private static int _nextInt;

        #endregion Fields

        #region Properties

        public int ID { get; private set; }

        public Component Sender { get; set; }
        public Component Target { get; set; }
        public float Damage { get; set; }

        public UnityEvent<Hitbox> DealedHit { get; } = new();

        #endregion Properties

        #region Public

        public void SetNextID()
        {
            ID = _nextInt++;
        }

        #endregion Public

        #region Unity

        protected void Awake()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer("Attack");
            Sender ??= this;
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
    }
}