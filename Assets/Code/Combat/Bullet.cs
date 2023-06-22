using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SphereCollider))]
    public class Bullet : Attack
    {
        #region fields

        private SphereCollider _collider;

        private static LinkedList<Bullet> _bullets = new();
        private static GameObject _bulletPrefab;
        private static Transform _attacksPool;

        #endregion fields

        #region Properties

        public SphereCollider Collider => _collider ??= GetComponent<SphereCollider>();
        public float Lifetime { get; private set; }

        #endregion Properties

        #region Public

        public static Bullet Spawn(
            Component sender,
            Vector3 position,
            float damage,
            float lifetime = 0.5F,
            float radius = 1F,
            Transform parent = null,
            bool isWorldSpace = false)
        {
            Bullet attack = _bullets.First?.Value;
            if (attack != null)
                _bullets.RemoveFirst();
            else
                attack = Instantiate(_bulletPrefab ??= Resources.Load<GameObject>("Prefabs/Combat/Bullet"))
                    .GetComponent<Bullet>();
            attack.gameObject.SetActive(true);
            attack.Sender = sender;
            attack.Lifetime = lifetime;
            attack.Collider.radius = radius;
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

        protected void OnEnable()
        {
            StartCoroutine(FadeOff(Lifetime));
        }

        protected void Update()
        {
            transform.GetChild(0).localScale = Vector3.one * Collider.radius;
        }

        protected void OnDisable()
        {
            _bullets.AddFirst(this);
        }

        protected void OnDestroy()
        {
            _bullets.Remove(this);
        }

        #endregion Unity

        #region Private

        private IEnumerator FadeOff(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            transform.SetParent(_attacksPool ??= new GameObject("BulletsPool").transform);
            gameObject.SetActive(false);
        }

        #endregion Private
    }
}