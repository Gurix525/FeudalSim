using Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        #region Fields

        private Hitbox[] _hitboxes;
        private Component _receiver;

        #endregion Fields

        #region Properties

        public float CurrentHealth { get; private set; } = 10F;

        public Component Receiver
        {
            get => _receiver;
            set
            {
                if (_hitboxes == null)
                    GetHitboxesInChildren();
                _receiver = value;
                foreach (Hitbox hitbox in _hitboxes)
                    hitbox.Receiver = value;
            }
        }

        public UnityEvent<Attack> GotHit { get; } = new();

        #endregion Properties

        #region Unity

        private void Awake()
        {
            GetHitboxesInChildren();
        }

        private void OnEnable()
        {
            foreach (var hitbox in _hitboxes)
                hitbox.GotHit.AddListener(OnGotHit);
        }

        private void OnDisable()
        {
            foreach (var hitbox in _hitboxes)
                hitbox.GotHit.RemoveAllListeners();
        }

        #endregion Unity

        #region Private

        private void GetHitboxesInChildren()
        {
            _hitboxes = GetComponentsInChildren<Hitbox>();
        }

        private void OnGotHit(Attack attack)
        {
            CurrentHealth -= attack.Damage;
            CurrentHealth = CurrentHealth.Clamp(0F, float.PositiveInfinity);
            GotHit.Invoke(attack);
        }

        #endregion Private
    }
}