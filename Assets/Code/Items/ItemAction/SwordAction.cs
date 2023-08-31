using UnityEngine;
using PlayerControls;
using Combat;
using Cursor = Controls.Cursor;
using System;
using TaskManager;
using System.Collections;
using Extensions;

namespace Items
{
    public class SwordAction : ItemAction
    {
        #region Fields

        private float _attackTime = 0.35F;

        private bool _isNextAttackQueued = false;

        private System.Random _random = new();

        #endregion Fields

        #region Properties

        private float Randomization => _random.NextFloat(0.9F, 1.1F);

        #endregion Properties

        #region Public

        public override void OnLeftMouseButton()
        {
            if (Player.Stats.CurrentStamina <= 0F)
                return;
            if (!IsLeftClickPermitted)
            {
                if (PlayerMovement.IsPendingAttack)
                    _isNextAttackQueued = true;
                return;
            }
            PlayerMovement.AttackComboNumber = 0;
            Attack();
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
        }

        #endregion Public

        #region Protected

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/Sword");
        }

        #endregion Protected

        #region Private

        private void Attack()
        {
            if (Cursor.ClearRaycastHit == null)
                return;
            Vector3 direction =
                (Cursor.ClearRaycastHit.Value.point - Player.transform.position)
                .normalized;
            PlayerMovement.RotateToCursor();
            CreateAttack(direction);
            TriggerVFX();
            MovePlayer(direction);
            Player.Stats.CurrentStamina -= 20F;
        }

        private void CreateAttack(Vector3 direction)
        {
            float swordsModifier = Player.Stats.GetSkill("Swords").Modifier;
            Attack attack = Bullet.Spawn(
                Player,
                Vector3.zero,
                Randomization * (4F + 4F * swordsModifier),
                _attackTime,
                1.25F + 0.25F * swordsModifier,
                Player.transform,
                false,
                IncreaseSwordsSkill);
            attack.transform.localRotation = Quaternion.identity;
            attack.SetNextID();
        }

        private void TriggerVFX()
        {
            GameObject slash = PlayerMovement.AttackComboNumber switch
            {
                0 => Player.VFX.FirstSlash,
                _ => Player.VFX.SecondSlash
            };
            slash.SetActive(false);
            slash.SetActive(true);
        }

        private void MovePlayer(Vector3 direction)
        {
            new Task(MovePlayerCoroutine(direction));
        }

        private void SetPlayerVelocity(Vector3 direction)
        {
            float ySpeed = PlayerMovement.Rigidbody.velocity.y;
            float xSpeed = direction.x * PlayerMovement.SprintSpeed * PlayerMovement.AttackMoveSpeedMultiplier;
            float zSpeed = direction.z * PlayerMovement.SprintSpeed * PlayerMovement.AttackMoveSpeedMultiplier;
            PlayerMovement.Rigidbody.velocity = new(xSpeed, ySpeed, zSpeed);
        }

        private void IncreaseSwordsSkill()
        {
            Player.Instance.Stats.AddSkill("Swords", 1F);
        }

        private IEnumerator MovePlayerCoroutine(Vector3 direction)
        {
            PlayerMovement.IsPendingAttack = true;
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
                PlayerMovement.AttackComboNumber = PlayerMovement.AttackComboNumber == 0 ? 1 : 0;
                Attack();
                yield break;
            }
            PlayerMovement.IsPendingAttack = false;
        }

        #endregion Private
    }
}