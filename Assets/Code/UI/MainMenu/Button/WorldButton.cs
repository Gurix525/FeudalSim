using TMPro;
using UnityEngine;

namespace UI
{
    public class WorldButton : Button
    {
        [SerializeField] private TextMeshProUGUI _nameText;

        public TextMeshProUGUI NameText => _nameText;

        protected override void Execute()
        {
        }
    }
}