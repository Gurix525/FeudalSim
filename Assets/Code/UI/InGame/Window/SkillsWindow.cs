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

        public void OpenClose()
        {
            gameObject.SetActive(gameObject.activeSelf ^ true);
        }

        private void Awake()
        {
            _slotPrefab = Resources.Load<SkillSlot>("Prefabs/UI/SkillSlot");
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Player.Current.Stats.StatsChanged.AddListener(ReloadSlots);
            Player.Current.Stats.ReloadStats();
        }

        private void OnDisable()
        {
            Player.Current.Stats.StatsChanged.RemoveListener(ReloadSlots);
        }

        private void ReloadSlots(Stats stats)
        {
            if (!_hasInitialized)
            {
                foreach (var skill in stats.Skills)
                {
                    var slot = Instantiate(_slotPrefab, transform);
                    _slots.Add(slot);
                }
                _hasInitialized = true;
            }
            int index = 0;
            foreach (var skill in stats.Skills)
            {
                _slots[index].Set(skill);
                index++;
            }
        }
    }
}