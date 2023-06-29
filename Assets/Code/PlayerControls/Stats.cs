using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [field: SerializeField] public float CurrentHP { get; private set; }
    [field: SerializeField] public float MaxHP { get; private set; }
    [field: SerializeField] public float CurrentStamina { get; private set; }
    [field: SerializeField] public float MaxStamina { get; private set; }

    private Dictionary<string, Skill> _skills = new()
    {
        { "Runing", 0 },
        { "Jumping", 0 },
        { "Woodcutting", 0 },
        { "Diggind", 0 },
        { "Sword", 0 },
        { "Parrying", 0 },
        { "Evading", 0 }
    };

    private class Skill
    {
        public string Name { get; }
        public int Level { get; private set; }
        public int CurrentXP { get; private set; }
    }
}