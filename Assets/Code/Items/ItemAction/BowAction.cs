using System.Collections;
using Controls;
using StarterAssets;
using TaskManager;
using UnityEngine;

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
            Player.Instance.ThirdPersonController.StartStringingBow();
        }

        public override void OnLeftMouseButtonRelase()
        {
            Player.Instance.ThirdPersonController.ShootBow();
        }

        public override void OnRightMouseButton()
        {
            Player.Instance.ThirdPersonController.RelaseBow();
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
        }
    }
}