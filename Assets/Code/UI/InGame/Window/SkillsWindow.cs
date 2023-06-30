using System.Collections.Generic;
using PlayerControls;
using UnityEngine;

namespace UI
{
    public class SkillsWindow : Window
    {
        private List<SkillSlot> _slots = new();
        private SkillSlot _slotPrefab;

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
            for (int i = 0; i < _slots.Count; i++)
            {
                Destroy(_slots[i].gameObject);
            }
            foreach (var skill in skills)
            {
                var slot = Instantiate(_slotPrefab, transform);
                slot.Initialize(skill);
                _slots.Add(slot);
            }
        }
    }
}