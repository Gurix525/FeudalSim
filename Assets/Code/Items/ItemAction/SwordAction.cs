﻿using System.Collections;
using Controls;
using StarterAssets;
using TaskManager;
using UnityEngine;
using PlayerControls;

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
            //Player.Instance.GetComponent<PlayerMovement>().StartAttack();
        }

        public override void OnMouseExit(Component component)
        {
            base.OnMouseExit(component);
        }
    }
}