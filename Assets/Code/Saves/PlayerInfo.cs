using System;
using Misc;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class PlayerInfo
    {
        public Vector3 Position;

        public PlayerInfo()
        {
        }

        public PlayerInfo(bool hasToInitialize)
        {
            if (hasToInitialize)
                Initialize();
        }

        public void Initialize()
        {
            Position = References.GetReference("Player").transform.position;
        }
    }
}