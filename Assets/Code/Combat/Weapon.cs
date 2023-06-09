using System;
using Controls;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        private Attack _attack;

        private void Awake()
        {
            _attack = GetComponentInChildren<Attack>(true);
            if (_attack == null)
                throw new MissingComponentException(
                    $"Nie utworzono obiektu attack jako dziecko broni {gameObject.name}");
            _attack.Damage = 4F;
            _attack.gameObject.SetActive(false);
        }

        private void Start()
        {
            _attack.Sender = Player.Instance;
            Player.PendingAttack.AddListener(OnPendingAttack);
        }

        private void OnPendingAttack(bool state)
        {
            _attack.gameObject.SetActive(state);
        }
    }
}