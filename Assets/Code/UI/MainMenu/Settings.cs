using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private VolumeProfile _profile;

        public VolumeProfile Profile => _volume != null ? _volume.profile : _profile;

        private Option[] _options;
        private string _settingsPath;

        private void Awake()
        {
            _settingsPath = Path.Combine(Application.persistentDataPath, "Settings.txt");
            _options = GetComponentsInChildren<Option>();
            InitializeAllOptions();
        }

        private void OnEnable()
        {
            LoadFromFile();
        }

        private void OnDisable()
        {
            SaveToFile();
        }

        private void InitializeAllOptions()
        {
            foreach (var option in _options)
                option.Initialize(this);
        }

        private void ExecuteAllOptions()
        {
            foreach (var option in _options)
                option.Execute();
        }

        private void SaveToFile()
        {
            string json = JsonUtility.ToJson(new SettingsInfo(_options));
            File.WriteAllText(_settingsPath, json);
        }

        private void LoadFromFile()
        {
            try
            {
                SettingsInfo settingsInfo = JsonUtility.FromJson<SettingsInfo>(
                File.ReadAllText(_settingsPath));
                foreach (var optionInfo in settingsInfo.OptionInfos)
                {
                    SetOption(optionInfo);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + e.StackTrace);
            }
            finally
            {
                ExecuteAllOptions();
            }
        }

        private void SetOption(OptionInfo optionInfo)
        {
            _options
                .First(option => option.Name == optionInfo.Name)
                .Value = optionInfo.Value;
        }

        [Serializable]
        private class SettingsInfo
        {
            public OptionInfo[] OptionInfos;

            public SettingsInfo(Option[] options)
            {
                OptionInfos = options
                    .Select(option => new OptionInfo(option.Name, option.Value))
                    .ToArray();
            }
        }

        [Serializable]
        private class OptionInfo
        {
            public string Name;
            public string Value;

            public OptionInfo(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }
    }
}