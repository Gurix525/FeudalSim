using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Saves;
using UnityEngine;

namespace UI
{
    public class WorldList : MonoBehaviour
    {
        private FileSystemWatcher _watcher;
        private DirectoryInfo _savesFolder;
        private bool _isToBeUpdated;

        private void Awake()
        {
            _savesFolder = Directory.CreateDirectory(
                Path.Combine(Application.persistentDataPath, "Saves"));
            InitializeWatcher();
            UpdateWorldList();
        }

        private void Update()
        {
            if (!_isToBeUpdated)
                return;
            _isToBeUpdated = false;
            UpdateWorldList();
        }

        private void InitializeWatcher()
        {
            _watcher = new(_savesFolder.FullName);
            _watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            _watcher.Changed += MarkForUpdate;
            _watcher.Created += MarkForUpdate;
            _watcher.Deleted += MarkForUpdate;
            _watcher.Filter = "*.zip";
            _watcher.EnableRaisingEvents = true;
        }

        private void MarkForUpdate(object sender = null, FileSystemEventArgs e = null)
        {
            _isToBeUpdated = true;
        }

        private void UpdateWorldList(object sender = null, FileSystemEventArgs e = null)
        {
            try
            {
                _isToBeUpdated = false;
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
                foreach (var zip in _savesFolder.GetFiles()
                    .Where(file => file.Name.EndsWith(".zip"))
                    .OrderByDescending(file => file.LastWriteTime.Ticks))
                {
                    GameObject worldButton = Instantiate(
                        Resources.Load<GameObject>("Prefabs/UI/WorldButton"),
                        transform);
                    var worldButtonComponent = worldButton.GetComponent<WorldButton>();
                    AssignWorldButtonData(zip, worldButtonComponent);
                }
            }
            catch (Exception es)
            {
                Debug.LogError(es.Message);
            }
        }

        private void AssignWorldButtonData(FileInfo file, WorldButton button)
        {
            using ZipArchive zip = ZipFile.Open(file.FullName, ZipArchiveMode.Read);
            string worldFilePath = Path.Combine(
                Application.persistentDataPath, "World.txt");
            foreach (ZipArchiveEntry entry in zip.Entries)
                if (entry.Name == "World.txt")
                    entry.ExtractToFile(worldFilePath);
            WorldInfo worldInfo = JsonUtility
                .FromJson<WorldInfo>(File.ReadAllText(worldFilePath));
            DateTime creationTime = new(worldInfo.CreationTime);
            DateTime lastPlayedTime = new(worldInfo.LastPlayedTime);
            TimeSpan fullTime = new(worldInfo.FullTimeInWorld);
            button.NameText.text = worldInfo.Name;
            button.CreationTimeText.text = creationTime.ToString("dd MMMM yyyy");
            button.LastPlayedTime.text = lastPlayedTime.ToString("dd MMMM yyyy HH:mm:ss");
            button.FullTimeInWorld.text =
                ((int)fullTime.TotalHours).ToString("00") + ":" + fullTime.Minutes.ToString("00") + ":" + fullTime.Seconds.ToString("00");
            File.Delete(worldFilePath);
        }
    }
}