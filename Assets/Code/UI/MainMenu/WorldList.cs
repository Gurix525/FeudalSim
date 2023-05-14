using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
                .Where(x => x.Name.EndsWith(".zip")))
                {
                    GameObject worldButton = Instantiate(
                        Resources.Load<GameObject>("Prefabs/UI/WorldButton"),
                        transform);
                    worldButton.GetComponent<WorldButton>().NameText.text = zip.Name[..^4];
                }
            }
            catch (Exception es)
            {
                Debug.LogError(es.Message);
            }
        }
    }
}