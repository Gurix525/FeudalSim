using System;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class TreeInfo : GameObjectInfo
    {
        public TreeInfo(Nature.Tree tree) : base(tree)
        {
        }
    }
}