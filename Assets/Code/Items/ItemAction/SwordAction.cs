using UnityEngine;
using PlayerControls;
using Combat;
using Cursor = Controls.Cursor;
using System;
using TaskManager;
using System.Collections;

namespace Items
{
    public class SwordAction : ItemAction
    {
        private float _attackTime = 0.4F;

        private bool _isNextAttackQueued = false;

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/Sword");
        }

        public override void OnLeftMouseButton()
        {
            if (!IsLeftClickPermitted)
            {
                if (_playerMovement.IsPendingAttack)
                    _isNextAttackQueued = true;
                return;
            }
            _playerMovement.AttackComboNumber = 0;
            Attack();
        }

        private void Attack()
        {
            if (Cursor.ClearRaycastHit == null)
                return;
            Vector3 direction =
                (Cursor.ClearRaycastHit.Value.point - _player.transform.position)
                .normalized;
            _playerMovement.RotateToCursor();
            CreateAttack(direction);
            MovePlayer(direction);
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
        }

        private void CreateAttack(Vector3 direction)
        {
            Attack attack = Bullet.Spawn(
                _player,
                _player.transform.position + direction + Vector3.up,
                4F,
                _attackTime,
                1.5F,
                _player.transform,
                true);
            attack.SetNextID();
        }

        private void MovePlayer(Vector3 direction)
        {
            new Task(MovePlayerCoroutine(direction));
        }

        private IEnumerator MovePlayerCoroutine(Vector3 direction)
        {
            _playerMovement.IsPendingAttack = true;
            float elapsedTime = 0F;
            while (elapsedTime < _attackTime / 2F)
            {
                elapsedTime += Time.fixedDeltaTime;
                SetPlayerVelocity(direction);
                yield return new WaitForFixedUpdate();
            }
            elapsedTime = 0F;
            SetPlayerVelocity(Vector3.zero);
            while (elapsedTime < _attackTime / 2F)
            {
                elapsedTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            if (_isNextAttackQueued)
            {
                _isNextAttackQueued = false;
                _playerMovement.AttackComboNumber = _playerMovement.AttackComboNumber == 0 ? 1 : 0;
                Attack();
                yield break;
            }
            _playerMovement.IsPendingAttack = false;
        }

        private void SetPlayerVelocity(Vector3 direction)
        {
            float ySpeed = _playerMovement.Rigidbody.velocity.y;
            float xSpeed = direction.x * _playerMovement.SprintSpeed * _playerMovement.AttackMoveSpeedMultiplier;
            float zSpeed = direction.z * _playerMovement.SprintSpeed * _playerMovement.AttackMoveSpeedMultiplier;
            _playerMovement.Rigidbody.velocity = new(xSpeed, ySpeed, zSpeed);
        }
    }
}