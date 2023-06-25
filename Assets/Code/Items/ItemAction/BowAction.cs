﻿using UnityEngine;
using Cursor = Controls.Cursor;

namespace Items
{
    public class BowAction : ItemAction
    {
        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/Bow");
        }

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
            Vector3 targetPosition = Cursor.ClearRaycastHit.Value.point;
            Vector3 playerPosition = _player.transform.position;
            _player.AimCurve.SetControlPoints(
                playerPosition + Vector3.up,
                targetPosition,
                (playerPosition + targetPosition) / 2F + Vector3.up * 2F);
        }

        public override void OnLeftMouseButtonRelase()
        {
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
    }
}