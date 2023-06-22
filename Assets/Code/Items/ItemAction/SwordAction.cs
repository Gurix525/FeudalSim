using UnityEngine;
using PlayerControls;
using Combat;
using Cursor = Controls.Cursor;

namespace Items
{
    public class SwordAction : ItemAction
    {
        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/Sword");
        }

        public override void OnLeftMouseButton()
        {
            if (Cursor.ClearRaycastHit == null)
                return;
            Player player = Player.Instance;
            Vector3 direction =
                (Cursor.ClearRaycastHit.Value.point - player.transform.position)
                .normalized;
            Attack attack = Bullet.Spawn(
                player,
                player.transform.position + direction + Vector3.up,
                4F,
                0.5F,
                1.5F,
                player.transform,
                true);
            attack.SetNextID();
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
        }
    }
}