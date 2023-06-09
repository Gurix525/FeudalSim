﻿using System.Collections;
using Controls;
using StarterAssets;
using TaskManager;
using UnityEngine;

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
            Player.Instance.GetComponent<ThirdPersonController>().StartAttack();
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
        }
    }
}