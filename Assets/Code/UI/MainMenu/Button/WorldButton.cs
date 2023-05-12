using System.Threading.Tasks;
using Misc;
using Saves;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace UI
{
    public class WorldButton : Button
    {
        [SerializeField] private TextMeshProUGUI _nameText;

        public TextMeshProUGUI NameText => _nameText;

        protected override void Execute()
        {
            _ = LoadWorld();
        }

        private async Task LoadWorld()
        {
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
            while (!sceneLoading.isDone)
                await Task.Yield();
            GameLoader gameLoader = new(_nameText.text);
            gameLoader.LoadGame();
        }
    }
}