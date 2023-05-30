using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        #region Fields

        private Hitbox[] _hitboxes;

        #endregion Fields

        #region Properties

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
            if (attack.Sender.TryGetComponent(out Health senderHealth))
                if (senderHealth == this)
                    return;
            GotHit.Invoke(attack);
        }

        #endregion Private
    }
}