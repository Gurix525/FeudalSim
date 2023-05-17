using System;
using Controls;
using World;

namespace Saves
{
    [Serializable]
    public class WorldInfo
    {
        public string Name;
        public long Seed;
        public long CreationTime;
        public long FullTimeInWorld;
        public long LastPlayedTime;

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
            Name = Controls.GameManager.WorldName;
            Seed = NoiseSampler.Seed;
            CreationTime = GameManager.WorldCreationTime.Ticks;
            FullTimeInWorld = GameManager.FullTimeInWorld.Ticks;
            LastPlayedTime = GameManager.LastPlayedTime.Ticks;
        }
    }
}