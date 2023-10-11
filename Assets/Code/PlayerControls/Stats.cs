using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Extensions;
using PlayerControls;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    #region Fields

    private Dictionary<string, Skill> _skills = new()
    {
        { "Running", Skill.Zero },
        { "Jumping", Skill.Zero },
        { "Woodcutting", Skill.Zero },
        { "Digging", Skill.Zero },
        { "Swords", Skill.Zero },
        { "Bows", Skill.Zero },
        { "Parrying", Skill.Zero }
    };

    private float _currentHP = 10F;
    private float _maxHP = 10F;
    private float _currentStamina = 100F;
    private float _maxStamina = 100F;
    private float _staminaRegenerationSpeed = 20F;
    private float _staminaRegenerationBlockTime = 0.5F;

    private float _timeSinceStaminaSubtraction;

    #endregion Fields

    #region Properties

    public UnityEvent<Stats> StatsChanged { get; } = new();
    public UnityEvent StaminaDepleted { get; } = new();

    public UnityEvent<string, Skill> SkillLevelIncreased { get; } = new();

    public float CurrentHP
    {
        get => _currentHP;
        set
        {
            _currentHP = value.Clamp(0F, MaxHP);
            StatsChanged.Invoke(this);
        }
    }

    public float MaxHP
    {
        get => _maxHP;
        private set
        {
            _maxHP = value;
            StatsChanged.Invoke(this);
        }
    }

    public float CurrentStamina
    {
        get => _currentStamina;
        set
        {
            if (_currentStamina > value)
                _timeSinceStaminaSubtraction = 0F;
            _currentStamina = value.Clamp(0F, MaxStamina);
            StatsChanged.Invoke(this);
            if (_currentStamina <= 0F)
                StaminaDepleted.Invoke();
        }
    }

    public float MaxStamina
    {
        get => _maxStamina;
        private set
        {
            _maxStamina = value;
            StatsChanged.Invoke(this);
        }
    }

    public IReadOnlyDictionary<string, Skill> Skills => _skills;

    #endregion Properties

    #region Public

    public void ReloadStats()
    {
        StatsChanged.Invoke(this);
    }

    public Skill GetSkill(string skill)
    {
        return _skills[skill];
    }

    public void AddSkill(string skill, float xp)
    {
        int previousLevel = _skills[skill].Level;
        _skills[skill] += xp;
        int currentLevel = _skills[skill].Level;
        if (currentLevel > previousLevel)
            SkillLevelIncreased.Invoke(skill, _skills[skill]);
        StatsChanged.Invoke(this);
    }

    #endregion Public

    #region Unity

    private void Start()
    {
        StatsChanged.Invoke(this);
        StartCoroutine(InvokeStatsChanged());
    }

    private void FixedUpdate()
    {
        _timeSinceStaminaSubtraction += Time.fixedDeltaTime;
        RegenerateStamina();
    }

    #endregion Unity

    #region Private

    private void RegenerateStamina()
    {
        if (CurrentStamina == MaxStamina)
            return;
        if (_timeSinceStaminaSubtraction < _staminaRegenerationBlockTime)
            return;
        CurrentStamina += Time.fixedDeltaTime * _staminaRegenerationSpeed;
    }

    private IEnumerator InvokeStatsChanged()
    {
        yield return null;
        StatsChanged.Invoke(this);
    }

    #endregion Private
}