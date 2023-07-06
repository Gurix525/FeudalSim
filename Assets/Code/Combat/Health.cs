using System.Collections.Generic;
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
        private Dictionary<int, int> AlreadyUsedIDs = new();

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
            if (AlreadyUsedIDs.ContainsKey(attack.ID))
                return;
            AlreadyUsedIDs[attack.ID] = attack.ID;
            //if (attack.Damage >= 0.9F * CurrentHealth)
            //    attack.Damage = CurrentHealth;
            CurrentHealth -= attack.Damage;
            CurrentHealth = CurrentHealth.Clamp(0F, float.PositiveInfinity);
            if (attack.OnDamageDealt != null)
            {
                attack.OnDamageDealt();
                attack.OnDamageDealt = null;
            }
            GotHit.Invoke(attack);
        }

        #endregion Private
    }
}