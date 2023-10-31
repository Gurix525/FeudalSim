using System.Collections;
using Extensions;
using PlayerControls;
using TaskManager;
using UnityEngine;

namespace Combat
{
    public class NormalAttackCombatAction : CombatAction
    {
        #region Fields

        private float _attackTime = 0.35F;

        private bool _isNextAttackQueued = false;

        private System.Random _random = new();

        #endregion Fields

        #region Properties

        private float RandomStrength => _random.NextFloat(0.9F, 1.1F);

        #endregion Properties

        #region Public

        public override void Execute()
        {
            if (Player.Current.Stats.CurrentStamina <= 0F)
                return;
            if (!PlayerMovement.Current.CanAttack)
            {
                if (PlayerMovement.Current.IsPendingAttack)
                    _isNextAttackQueued = true;
                return;
            }
            PlayerMovement.Current.AttackComboNumber = 0;
            Attack();
        }

        #endregion Public

        #region Private

        private void Attack()
        {
            PlayerMovement.Current.RotateToCursor();
            CreateAttack(PlayerMovement.Current.transform.forward);
            TriggerVFX();
            MovePlayer(PlayerMovement.Current.transform.forward);
            Player.Current.Stats.CurrentStamina -= 20F;
        }

        private void CreateAttack(Vector3 direction)
        {
            float swordsModifier = Player.Current.Stats.GetSkill("Swords").Modifier;
            Attack attack = Bullet.Spawn(
                Player.Current,
                Vector3.zero,
                RandomStrength * (4F + 4F * swordsModifier),
                _attackTime,
                1.25F + 0.25F * swordsModifier,
                Player.Current.transform,
                false,
                IncreaseSwordsSkill);
            attack.transform.localRotation = Quaternion.identity;
            attack.SetNextID();
        }

        private void TriggerVFX()
        {
            GameObject slash = PlayerMovement.Current.AttackComboNumber switch
            {
                0 => Player.Current.VFX.FirstSlash,
                _ => Player.Current.VFX.SecondSlash
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
            float ySpeed = PlayerMovement.Current.Rigidbody.velocity.y;
            float xSpeed = direction.x * PlayerMovement.Current.SprintSpeed * PlayerMovement.Current.AttackMoveSpeedMultiplier;
            float zSpeed = direction.z * PlayerMovement.Current.SprintSpeed * PlayerMovement.Current.AttackMoveSpeedMultiplier;
            PlayerMovement.Current.Rigidbody.velocity = new(xSpeed, ySpeed, zSpeed);
        }

        private void IncreaseSwordsSkill()
        {
            Player.Current.Stats.AddSkill("Swords", 1F);
        }

        private IEnumerator MovePlayerCoroutine(Vector3 direction)
        {
            PlayerMovement.Current.IsPendingAttack = true;
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
                PlayerMovement.Current.AttackComboNumber = PlayerMovement.Current.AttackComboNumber == 0 ? 1 : 0;
                Attack();
                yield break;
            }
            PlayerMovement.Current.IsPendingAttack = false;
        }

        #endregion Private
    }
}