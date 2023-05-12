using Misc;
using World;

namespace Saves
{
    public class GameLoader
    {
        private string _worldName;

        public GameLoader(string worldName)
        {
            _worldName = worldName;
        }

        public void LoadGame()
        {
            //NoiseSampler.SetSeed(random.Next());
            GrassInstancer.MarkToReload();
            //References.GetReference("Player").transform.position = new(0F, originHeight, 0F);
        }
    }
}