using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UI
{
    public class WorldList : MonoBehaviour
    {
        private void Awake()
        {
            DirectoryInfo savesFolder = Directory.CreateDirectory(
                Path.Combine(Application.persistentDataPath, "Saves"));
            foreach (var zip in savesFolder.GetFiles()
                .Where(x => x.Name.EndsWith(".zip")))
            {
                GameObject worldButton = Instantiate(
                    Resources.Load<GameObject>("Prefabs/UI/WorldButton"),
                    transform);
                worldButton.GetComponent<WorldButton>().NameText.text = zip.Name[..^4];
            }
        }
    }
}