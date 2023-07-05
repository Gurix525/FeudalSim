using System.Collections.Generic;
using PlayerControls;
using UnityEngine;

namespace UI
{
    public class SkillsWindow : Window
    {
        private List<SkillSlot> _slots = new();
        private SkillSlot _slotPrefab;
        private bool _hasInitialized;

        private void Awake()
        {
            _slotPrefab = Resources.Load<SkillSlot>("Prefabs/UI/SkillSlot");
        }

        private void OnEnable()
        {
            Player.Instance.Stats.StatsChanged.AddListener(ReloadSlots);
            Player.Instance.Stats.ReloadStats();
        }

        private void OnDisable()
        {
            Player.Instance.Stats.StatsChanged.RemoveListener(ReloadSlots);
        }

        private void ReloadSlots(IReadOnlyDictionary<string, Skill> skills)
        {
            if (!_hasInitialized)
            {
                foreach (var skill in skills)
                {
                    var slot = Instantiate(_slotPrefab, transform);
                    _slots.Add(slot);
                }
                _hasInitialized = true;
            }
            int index = 0;
            foreach (var skill in skills)
            {
                _slots[index].Set(skill);
                index++;
            }
        }
    }
}