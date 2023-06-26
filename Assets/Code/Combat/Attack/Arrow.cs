using System;
using System.Collections;
using UnityEngine;

namespace Combat
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField]
        private Attack _attack;

        [SerializeField]
        private Rigidbody _rigidbody;

        private Vector3[] _path;

        public void Initialize(Vector3[] path, Component sender, float damage)
        {
            _path = path;
            _attack.Sender = sender;
            _attack.Damage = damage;
            _attack.DealedHit.AddListener(OnDealedHit);
            transform.position = _path[0];
            StartCoroutine(FollowPath());
        }

        private IEnumerator FollowPath()
        {
            int index = 1;
            while (index < _path.Length)
            {
                _rigidbody.MovePosition(_path[index]);
                yield return new WaitForFixedUpdate();
                index++;
            }
            Destroy(gameObject);
        }

        private void OnDealedHit(Hitbox hitbox)
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}