using System;
using System.Collections;
using Controls;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
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

                new TaskManager.Task(GenerateWorld());
            }
        }

        private IEnumerator GenerateWorld()
        {
            LoadingScreen.Enable();
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
            while (!sceneLoading.isDone)
                yield return null;
            yield return null;
            World.Terrain.Reset();
            NoiseSampler.SetSeed(_seedInput.Seed);
            GameManager.WorldName = _nameInput.Text;
            GameManager.WorldCreationTime = DateTime.Now.Ticks;
            GameManager.LastPlayedTime = DateTime.Now.Ticks;
            yield return TerrainRenderer.GenerateWorld(Vector2Int.zero);
            GrassInstancer.MarkToReload();
            float originHeight = World.Terrain.GetHeight(new(0F, 0F));
            var player = References.GetReference("Player");
            player.SetActive(false);
            player.transform.position = new(0F, originHeight + 2F, 0F);
            player.SetActive(true);
            LoadingScreen.Disable();
        }
    }
}