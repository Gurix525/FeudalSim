using System.Collections.Generic;
using PlayerControls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private Image _fill;

        public void Initialize(KeyValuePair<string, Skill> skill)
        {
            _name.text = skill.Key;
            _level.text = skill.Value.Level.ToString();
            float currentXP = skill.Value.CurrentXPSurplus;
            float requiredXP = skill.Value.RequiredXPToNextLevel;
            _counter.text = $"{(int)currentXP}/{(int)requiredXP}";
            _fill.type = Image.Type.Filled;
            _fill.fillMethod = Image.FillMethod.Horizontal;
            _fill.fillAmount = currentXP / requiredXP;
        }
    }
}