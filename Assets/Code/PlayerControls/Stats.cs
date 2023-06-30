using System;
using System.Collections.Generic;
using PlayerControls;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [field: SerializeField] public float CurrentHP { get; private set; }
    [field: SerializeField] public float MaxHP { get; private set; }
    [field: SerializeField] public float CurrentStamina { get; private set; }
    [field: SerializeField] public float MaxStamina { get; private set; }

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
}