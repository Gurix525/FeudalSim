﻿using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private bool _isLeftHanded = false;

        public bool IsLeftHanded => _isLeftHanded;
    }
}