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
        private static Transform _bulletsPool;

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
            Bullet bullet = _bullets.First?.Value;
            if (bullet != null)
                _bullets.RemoveFirst();
            else
                bullet = Instantiate(_bulletPrefab ??= Resources.Load<GameObject>("Prefabs/Combat/Bullet"))
                    .GetComponent<Bullet>();
            bullet.gameObject.SetActive(true);
            bullet.Sender = sender;
            bullet.Lifetime = lifetime;
            bullet.Collider.radius = radius;
            bullet.Damage = damage;
            bullet.transform.SetParent(parent);
            if (isWorldSpace)
                bullet.transform.position = position;
            else
                bullet.transform.localPosition = position;
            return bullet;
        }

        #endregion Public

        #region Unity

        protected void OnEnable()
        {
            StartCoroutine(FadeOff(Lifetime));
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
            transform.SetParent(_bulletsPool ??= new GameObject("BulletsPool").transform);
            gameObject.SetActive(false);
        }

        #endregion Private
    }
}