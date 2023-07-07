using System;
using System.Collections.Generic;
using Input;
using PlayerControls;
using UnityEngine;
using UnityEngine.InputSystem;

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
            PlayerController.MainSkills.AddListener(ActionType.Started, OpenClose);
            gameObject.SetActive(false);
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

        private void OpenClose(InputAction.CallbackContext context)
        {
            gameObject.SetActive(gameObject.activeSelf ^ true);
        }
    }
}