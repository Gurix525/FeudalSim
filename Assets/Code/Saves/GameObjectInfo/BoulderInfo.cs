using System;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class BoulderInfo : GameObjectInfo
    {
        public BoulderInfo(Nature.Boulder tree) : base(tree)
        {
        }
    }
}