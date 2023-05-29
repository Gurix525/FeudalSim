using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        private Hitbox[] _hitboxes;

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
        }

        #endregion Private
    }
}