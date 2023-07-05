using System;
using Combat;
using PlayerControls;
using UnityEngine;
using Cursor = Controls.Cursor;

namespace Items
{
    public class BowAction : ItemAction
    {
        #region Fields

        private GameObject _arrowPrefab;

        #endregion Fields

        #region Public

        public override void OnLeftMouseButton()
        {
            if (IsLeftClickPermitted)
            {
                _player.AimCurve.Enable();
                _playerMovement.IsStringingBow = true;
            }
        }

        public override void Update()
        {
            if (Cursor.ClearRaycastHit == null || !_playerMovement.IsStringingBow)
                return;
            var hit = Cursor.ClearRaycastHit.Value;
            Vector3 playerPosition = _player.transform.position;
            _player.AimCurve.SetControlPoints(
                _player.transform.position + Vector3.up * 1.3F,
                hit.point,
                hit.normal);
            _playerMovement.LeftHandIKGoal = _player.AimCurve.GetNodePosition(5);
        }

        public override void OnLeftMouseButtonRelase()
        {
            if (_player.AimCurve.IsCurveEnabled)
            {
                if (_player.AimCurve.Curve != null)
                    Arrow.Spawn(
                        _player.AimCurve.Curve,
                        _player,
                        4F,
                        IncreaseBowsSkill);
            }
            _player.AimCurve.Disable();
            _playerMovement.IsStringingBow = false;
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