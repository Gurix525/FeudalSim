using System;
using System.Collections;
using Maths;
using UnityEngine;

namespace Combat
{
    public class Arrow : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Attack _attack;

        [SerializeField]
        private Rigidbody _rigidbody;

        private Curve _curve;

        private float _elapsedTime = 0F;

        private static readonly float _moveSpeed = 20F;

        #endregion Fields

        #region Public

        public void Initialize(Curve curve, Component sender, float damage)
        {
            _curve = curve;
            _attack.Sender = sender;
            _attack.Damage = damage;
            _attack.DealedHit.AddListener(OnDealedHit);
            transform.position = curve.EvaluatePosition(0F);
        }

        #endregion Public

        #region Unity

        private void OnTriggerEnter(Collider collider)
        {
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            FollowCurve();
            DestroyIfTooLow();
        }

        #endregion Unity

        #region Private

        private void FollowCurve()
        {
            _elapsedTime += Time.fixedDeltaTime;
            _rigidbody.MovePosition(_curve.EvaluatePosition(_elapsedTime * _moveSpeed / _curve.ApproximateLength));
        }

        private void DestroyIfTooLow()
        {
            if (transform.position.y < -50F)
                Destroy(gameObject);
        }

        private void OnDealedHit(Hitbox hitbox)
        {
            Destroy(gameObject);
        }

        #endregion Private
    }
}