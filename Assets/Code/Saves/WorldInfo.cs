using System;
using World;

namespace Saves
{
    [Serializable]
    public class WorldInfo
    {
        public string Name;
        public long Seed;

        public WorldInfo()
        {
        }

        public WorldInfo(bool hasToInitialize)
        {
            if (hasToInitialize)
                Initialize();
        }

        public void Initialize()
        {
            Name = "A";
            Seed = NoiseSampler.Seed;
        }
    }
}