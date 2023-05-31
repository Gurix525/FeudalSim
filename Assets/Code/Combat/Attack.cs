using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class Attack : MonoBehaviour
    {
        #region fields

        private Collider _collider;

        private static LinkedList<Attack> _attacks = new();
        private static GameObject _attackPrefab;
        private static Transform _attacksPool;

        #endregion fields

        #region Properties

        public Component Sender { get; private set; }
        public float Lifetime { get; private set; }
        public float Damage { get; private set; }

        #endregion Properties

        #region Public

        public static Attack Spawn(
            Component sender,
            Vector3 position,
            float damage,
            float lifetime = 0.5F,
            Transform parent = null,
            bool isWorldSpace = false)
        {
            Attack attack = _attacks.First?.Value;
            if (attack == null)
            {
                attack = Instantiate(_attackPrefab ??= Resources.Load<GameObject>("Prefabs/Combat/Attack"))
                    .GetComponent<Attack>();
            }
            attack.gameObject.SetActive(true);
            attack.Sender = sender;
            attack.Lifetime = lifetime;
            attack.Damage = damage;
            attack.transform.SetParent(parent);
            if (isWorldSpace)
                attack.transform.position = position;
            else
                attack.transform.localPosition = position;
            return attack;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Attack");
            Sender = this;
        }

        private void OnEnable()
        {
            StartCoroutine(FadeOff(Lifetime));
        }

        private void OnDisable()
        {
            _attacks.AddFirst(this);
        }

        private void OnDestroy()
        {
            _attacks.Remove(this);
        }

        #endregion Unity

        #region Private

        private IEnumerator FadeOff(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            transform.SetParent(_attacksPool ??= new GameObject("AttacksPool").transform);
            gameObject.SetActive(false);
        }

        #endregion Private
    }
}