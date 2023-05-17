using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Controls;
using Misc;
using MyNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using World;

namespace UI
{
    public class CreateWorldButton : Button
    {
        [SerializeField] private NameInput _nameInput;
        [SerializeField] private SeedInput _seedInput;

        private bool _isBusy;

        protected override void Execute()
        {
            if (_nameInput.IsNameAllowed && !_isBusy)
            {
                _isBusy = true;
                _ = GenerateWorld();
            }
        }

        private async Task GenerateWorld()
        {
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
            while (!sceneLoading.isDone)
                await Task.Yield();
            NoiseSampler.SetSeed(_seedInput.Seed);
            GameManager.WorldName = _nameInput.Text;
            GameManager.WorldCreationTime = DateTime.Now;
            GameManager.LastPlayedTime = DateTime.Now;
            TerrainRenderer.GenerateWorld(Vector2Int.zero);
            GrassInstancer.MarkToReload();
            float originHeight = World.Terrain.GetHeight(new(0F, 0F));
            var player = References.GetReference("Player");
            player.SetActive(false);
            player.transform.position = new(0F, originHeight, 0F);
            player.SetActive(true);
        }
    }
}