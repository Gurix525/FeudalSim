using UnityEngine;

namespace UI
{
    public class ExitGameButton : Button
    {
        protected override void Execute()
        {
            Application.Quit();
        }
    }
}