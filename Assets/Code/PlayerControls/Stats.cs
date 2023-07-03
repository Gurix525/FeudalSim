using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlayerControls;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    #region Fields

    private Dictionary<string, Skill> _skills = new()
    {
        { "Running", new(32) },
        { "Jumping", new(10000)},
        { "Woodcutting", new(5664)},
        { "Digging", new(16)},
        { "Sword", new(648)},
        { "Parrying", new(51)},
        { "Evading", new(8898)}
    };

    #endregion Fields

    #region Properties

    public UnityEvent<IReadOnlyDictionary<string, Skill>> StatsChanged { get; } = new();
    public UnityEvent<string, Skill> SkillLevelIncreased { get; } = new();

    [field: SerializeField] public float CurrentHP { get; private set; }
    [field: SerializeField] public float MaxHP { get; private set; }
    [field: SerializeField] public float CurrentStamina { get; private set; }
    [field: SerializeField] public float MaxStamina { get; private set; }

    #endregion Properties

    #region Public

    public void ReloadStats()
    {
        StatsChanged.Invoke(_skills);
    }

    public Skill GetSkill(string skill)
    {
        return _skills[skill];
    }

    public void ModifySkill(string skill, float xp)
    {
        int previousLevel = _skills[skill].Level;
        _skills[skill] += xp;
        int currentLevel = _skills[skill].Level;
        if (currentLevel > previousLevel)
            SkillLevelIncreased.Invoke(skill, _skills[skill]);
        StatsChanged.Invoke(_skills);
    }

    #endregion Public

    #region Unity

    private void Start()
    {
        StatsChanged.Invoke(_skills);
    }

    #endregion Unity
}