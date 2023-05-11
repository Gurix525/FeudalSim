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
            Name = "A";
            Seed = NoiseSampler.Seed;
        }
    }
}