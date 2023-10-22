using System;
using Maths;
using Misc;
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

        [SerializeField]
        private TrailRenderer _trailRenderer;

        private BezierCurve _curve;

        private float _elapsedTime = 0F;

        private float _moveSpeed = 20F;

        private static int? _hitboxLayerMask;

        private static Pool<Arrow> _pool = new("Prefabs/Combat/Arrow");

        #endregion Fields

        #region Public

        public static Arrow Spawn(BezierCurve curve, Component sender, float damage, float moveSpeed = 20F, Action onDamageDealt = null)
        {
            Arrow arrow = _pool.Pull();
            arrow._elapsedTime = 0F;
            arrow._curve = curve;
            arrow._moveSpeed = moveSpeed;
            arrow.transform.position = curve.EvaluatePosition(0F);
            arrow._attack.Sender = sender;
            arrow._attack.Damage = damage;
            arrow._attack.DealedHit.AddListener(arrow.OnDealedHit);
            arrow._attack.SetNextID();
            arrow._trailRenderer.Clear();
            arrow._attack.OnDamageDealt = onDamageDealt;
            arrow.gameObject.SetActive(true);
            return arrow;
        }

        #endregion Public

        #region Unity

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.isTrigger)
                return;
            ReturnToPool();
        }

        private void FixedUpdate()
        {
            FollowCurve();
            DestroyIfTooLow();
        }

        protected void OnDestroy()
        {
            _pool.Remove(this);
        }

        #endregion Unity

        #region Private

        private void FollowCurve()
        {
            Vector3 previousPosition = transform.position;
            _elapsedTime += Time.fixedDeltaTime;
            Vector3 nextPosition = _curve.EvaluatePosition(
                    (_elapsedTime * _moveSpeed)
                    / _curve.ApproximateLength);
            bool oldQueriesSetings = Physics.queriesHitTriggers;
            Physics.queriesHitTriggers = true;
            var hitboxes = Physics.SphereCastAll(
                previousPosition,
                0.5F,
                (nextPosition - previousPosition).normalized,
                (nextPosition - previousPosition).magnitude,
                _hitboxLayerMask ??= LayerMask.GetMask("Hitbox")
                );
            Physics.queriesHitTriggers = oldQueriesSetings;
            int index = 0;
            while (gameObject.activeInHierarchy && index < hitboxes.Length)
            {
                _attack.ForceCollision(hitboxes[index].collider);
                hitboxes[index].collider.GetComponent<Hitbox>().ForceCollision(_attack.Collider);
                if (gameObject.activeInHierarchy == false)
                    return;
                index++;
            }
            if (Physics.Linecast(previousPosition, nextPosition))
            {
                ReturnToPool();
                return;
            }
            _rigidbody.MovePosition(nextPosition);
        }

        private void DestroyIfTooLow()
        {
            if (transform.position.y < -50F)
                ReturnToPool();
        }

        private void OnDealedHit(Hitbox hitbox, Vector3 contact)
        {
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            _pool.Push(this);
        }

        #endregion Private
    }
}