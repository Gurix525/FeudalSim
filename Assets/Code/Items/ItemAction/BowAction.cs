using System;
using Combat;
using Extensions;
using PlayerControls;
using UnityEngine;
using Cursor = Controls.Cursor;

namespace Items
{
    public class BowAction : ItemAction
    {
        #region Fields

        private GameObject _arrowPrefab;
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
            if (IsLeftClickPermitted)
            {
                Player.AimCurve.Enable();
                PlayerMovement.IsStringingBow = true;
            }
        }

        public override void Update()
        {
            if (Cursor.ClearRaycastHit == null || !PlayerMovement.IsStringingBow)
                return;
            var hit = Cursor.ClearRaycastHit.Value;
            Vector3 playerPosition = Player.transform.position;
            Player.AimCurve.SetControlPoints(
                Player.transform.position + Vector3.up * 1.3F,
                hit.point,
                hit.normal);
            PlayerMovement.LeftHandIKGoal = Player.AimCurve.GetNodePosition(5);
        }

        public override void OnLeftMouseButtonRelase()
        {
            if (Player.AimCurve.IsCurveEnabled)
            {
                if (Player.AimCurve.Curve != null)
                {
                    float bowsModifier = Player.Stats.GetSkill("Bows").Modifier;
                    Arrow.Spawn(
                        Player.AimCurve.Curve,
                        Player,
                        Randomization * (4F + 4F * bowsModifier),
                        20F + 10F * bowsModifier,
                        IncreaseBowsSkill);
                    Player.Stats.CurrentStamina -= 20F;
                }
            }
            Player.AimCurve.Disable();
            PlayerMovement.IsStringingBow = false;
        }

        public override void OnRightMouseButton()
        {
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
        }

        #endregion Public

        #region Protected

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/Bow");
        }

        #endregion Protected

        #region Private

        private void IncreaseBowsSkill()
        {
            Player.Instance.Stats.AddSkill("Bows", 1F);
        }

        #endregion Private
    }
}