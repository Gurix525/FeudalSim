using System.Linq;
using System.Threading.Tasks;
using Misc;
using MyNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using World;

namespace UI
{
    public class CreateWorldButton : Button
    {
        protected override void Execute()
        {
            _ = GenerateWorld();
        }

        private async Task GenerateWorld()
        {
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
            while (!sceneLoading.isDone)
                await Task.Yield();
            System.Random random = new();
            NoiseSampler.SetSeed(random.Next());
            TerrainRenderer.GenerateWorld(Vector2Int.zero);
            GrassInstancer.MarkToReload();
            float originHeight = World.Terrain.GetHeight(new(0F, 0F));
            References.GetReference("Player").transform.position = new(0F, originHeight, 0F);
        }
    }
}