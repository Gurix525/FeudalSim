using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            TerrainRenderer.GenerateWorld();
        }
    }
}