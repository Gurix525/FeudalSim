using System.IO;
using UnityEngine;

namespace UI
{
    public class DeleteWorldButton : Button
    {
        protected override void Execute()
        {
            var worldButton = GetComponentInParent<WorldButton>();
            string worldName = worldButton.NameText.text;
            worldButton.IsWorldDeleted = true;
            string worldPath = Path.Combine(Application.persistentDataPath, "Saves", worldName + ".zip");
            File.Delete(worldPath);
        }
    }
}