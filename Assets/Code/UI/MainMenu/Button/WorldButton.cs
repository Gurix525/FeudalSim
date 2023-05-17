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
        [SerializeField] private TextMeshProUGUI _creationTime;
        [SerializeField] private TextMeshProUGUI _lastPlayedTime;
        [SerializeField] private TextMeshProUGUI _fullTimeInWorld;

        public TextMeshProUGUI NameText => _nameText;
        public TextMeshProUGUI CreationTimeText => _creationTime;
        public TextMeshProUGUI LastPlayedTime => _lastPlayedTime;
        public TextMeshProUGUI FullTimeInWorld => _fullTimeInWorld;

        public bool IsWorldDeleted { get; set; }

        protected override void Execute()
        {
            _ = LoadWorld();
        }

        private async Task LoadWorld()
        {
            if (IsWorldDeleted)
                return;
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
            while (!sceneLoading.isDone)
                await Task.Yield();
            GameLoader gameLoader = new(_nameText.text);
            gameLoader.LoadGame();
        }
    }
}