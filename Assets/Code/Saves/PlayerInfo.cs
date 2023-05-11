using System;
using Misc;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class PlayerInfo
    {
        public Vector2 Position;

        public PlayerInfo()
        {
            Position = References.GetReference("Player").transform.position;
        }
    }
}