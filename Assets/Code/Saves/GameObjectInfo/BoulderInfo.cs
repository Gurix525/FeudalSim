using System;

namespace Saves
{
    [Serializable]
    public class BoulderInfo : GameObjectInfo
    {
        public BoulderInfo()
        {
        }

        public BoulderInfo(Nature.Boulder boulder) : base(boulder)
        {
            Initialize(boulder);
        }

        public void Initialize(Nature.Boulder boulder)
        {
            base.Initialize(boulder);
        }
    }
}