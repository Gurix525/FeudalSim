using System.IO;
using UnityEngine;

namespace UI
{
    public class WorldList : MonoBehaviour
    {
        private void Awake()
        {
            DirectoryInfo savesFolder = Directory.CreateDirectory(
                Path.Combine(Application.persistentDataPath, "Saves"));
            foreach (var directory in savesFolder.GetDirectories())
            {
                GameObject worldButton = Instantiate(
                    Resources.Load<GameObject>("Prefabs/UI/WorldButton"),
                    transform);
                worldButton.GetComponent<WorldButton>().NameText.text = directory.Name;
                Debug.Log("Tutaj wpisac nawzwe zeby sie dopowiedni ladowal");
            }
        }
    }
}