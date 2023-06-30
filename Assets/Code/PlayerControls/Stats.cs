using System;
using System.Collections.Generic;
using PlayerControls;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    private Dictionary<string, Skill> _skills = new()
    {
        { "Running", Skill.Zero },
        { "Jumping", Skill.Zero},
        { "Woodcutting", Skill.Zero },
        { "Digging", Skill.Zero },
        { "Sword", Skill.Zero },
        { "Parrying", Skill.Zero },
        { "Evading", Skill.Zero }
    };

    public UnityEvent<string, Skill> SkillLevelIncreased { get; } = new();

    [field: SerializeField] public float CurrentHP { get; private set; }
    [field: SerializeField] public float MaxHP { get; private set; }
    [field: SerializeField] public float CurrentStamina { get; private set; }
    [field: SerializeField] public float MaxStamina { get; private set; }

    public Skill GetSkill(string skill)
    {
        return _skills[skill];
    }

    public void IncreaseSkill(string skill, float xp)
    {
        if (xp <= 0F)
            throw new ArgumentException("Dodawany xp musi być dodatni.");
        int previousLevel = _skills[skill].Level;
        _skills[skill] += xp;
        int currentLevel = _skills[skill].Level;
        if (currentLevel > previousLevel)
            SkillLevelIncreased.Invoke(skill, _skills[skill]);
    }
}