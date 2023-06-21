using System.Collections;
using Controls;
using StarterAssets;
using TaskManager;
using UnityEngine;
using PlayerControls;
using Combat;

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
            Player player = Player.Instance;
            Bullet.Spawn(
                player,
                player.transform.forward,
                4F,
                0.5F,
                1F,
                player.transform,
                false);
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
        }
    }
}