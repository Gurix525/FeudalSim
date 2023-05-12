using System;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class TreeInfo : GameObjectInfo
    {
        public TreeInfo()
        {
        }

        public TreeInfo(Nature.Tree tree) : base(tree)
        {
            Initialize(tree);
        }

        public void Initialize(Nature.Tree tree)
        {
            base.Initialize(tree);
        }
    }
}