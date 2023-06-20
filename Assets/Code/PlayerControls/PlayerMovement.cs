using System;
using Extensions;
using Input;
using UnityEngine;

namespace PlayerControls
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private float _moveSpeed = 6F;

        [SerializeField]
        private float _acceleration = 10F;

        [SerializeField]
        private float _gravityForce = Physics.gravity.magnitude;

        private Rigidbody _rigidbody;

        private bool _isCursorRaycastNull = true;

        #endregion Fields

        #region Properties

        public bool CanMove => true;

        public bool IsGravityEnabled => true;

        public bool CanRotateToCursor => !_isCursorRaycastNull;

        #endregion Properties

        #region Unity

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            CheckConditions();
            if (CanMove)
                Move();
            if (IsGravityEnabled)
                DoGravity();
            if (CanRotateToCursor)
                RotateToCursor();
        }

        #endregion Unity

        #region Private

        private void CheckConditions()
        {
            _isCursorRaycastNull = Controls.Cursor.RaycastHit == null;
        }

        private void Move()
        {
            Vector2 direction = PlayerController.MainMove.ReadValue<Vector2>();
            direction *= _moveSpeed;
            float y = _rigidbody.velocity.y;
            _rigidbody.velocity = new(direction.x, y, direction.y);
            //if (correctedDirection != Vector3.zero)
            //    _rigidbody.AddForce(correctedDirection * _acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //else
            //    _rigidbody.AddForce(-_rigidbody.velocity * _acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange); ;
            //float y = _rigidbody.velocity.y;
            //Vector3 clampedVelocity = Vector3.ClampMagnitude(_rigidbody.velocity, _moveSpeed);
            //_rigidbody.velocity = new(clampedVelocity.x, y, clampedVelocity.z);
        }

        private void DoGravity()
        {
            _rigidbody.AddForce(Vector3.down * _gravityForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        private void RotateToCursor()
        {
            transform.LookAt(Controls.Cursor.RaycastHit.Value.point);
        }

        #endregion Private
    }
}