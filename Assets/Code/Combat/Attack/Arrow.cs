using System;
using System.Collections;
using System.Collections.Generic;
using AI;
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

        [SerializeField]
        private TrailRenderer _trailRenderer;

        private Curve _curve;

        private float _elapsedTime = 0F;

        private static readonly float _moveSpeed = 20F;

        private static LinkedList<Arrow> _arrows = new();
        private static GameObject _arrowPrefab;
        private static Transform _arrowsPool;

        #endregion Fields

        #region Public

        public static Arrow Spawn(Curve curve, Component sender, float damage)
        {
            Arrow arrow = _arrows.First?.Value;
            if (arrow != null)
                _arrows.RemoveFirst();
            else
                arrow = Instantiate(_arrowPrefab ??= Resources.Load<GameObject>("Prefabs/Combat/Arrow"))
                    .GetComponent<Arrow>();
            arrow._elapsedTime = 0F;
            arrow._curve = curve;
            arrow.transform.position = curve.EvaluatePosition(0F);
            arrow._attack.Sender = sender;
            arrow._attack.Damage = damage;
            arrow._attack.DealedHit.AddListener(arrow.OnDealedHit);
            arrow._attack.SetNextID();
            arrow._trailRenderer.Clear();
            arrow.gameObject.SetActive(true);
            return arrow;
        }

        #endregion Public

        #region Unity

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.isTrigger)
                return;
            transform.SetParent(_arrowsPool ??= new GameObject("ArrowsPool").transform);
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            FollowCurve();
            DestroyIfTooLow();
        }

        protected void OnDisable()
        {
            _arrows.AddFirst(this);
        }

        protected void OnDestroy()
        {
            _arrows.Remove(this);
        }

        #endregion Unity

        #region Private

        private void FollowCurve()
        {
            _elapsedTime += Time.fixedDeltaTime;
            _rigidbody.MovePosition(_curve.EvaluatePosition(
                    _elapsedTime * _moveSpeed / _curve.ApproximateLength));
        }

        private void DestroyIfTooLow()
        {
            if (transform.position.y < -50F)
            {
                transform.SetParent(_arrowsPool ??= new GameObject("ArrowsPool").transform);
                gameObject.SetActive(false);
            }
        }

        private void OnDealedHit(Hitbox hitbox)
        {
            transform.SetParent(_arrowsPool ??= new GameObject("ArrowsPool").transform);
            gameObject.SetActive(false);
        }

        #endregion Private
    }
}