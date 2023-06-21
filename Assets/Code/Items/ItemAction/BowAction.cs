using System.Collections;
using Controls;
using StarterAssets;
using TaskManager;
using UnityEngine;
using PlayerControls;

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
        }

        public override void OnLeftMouseButtonRelase()
        {
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