using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WorldUI
{
    public class AIHealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Image _background;

        private float _timeSinceHealthChange = 5F;

        public void Initialize(Stats stats, Vector3 offset)
        {
            transform.localPosition = offset;
            stats.StatsChanged.AddListener(SetFill);
            StartCoroutine(DisableBar());
        }

        private void FixedUpdate()
        {
            _timeSinceHealthChange += Time.fixedDeltaTime;
            if (_timeSinceHealthChange <= 5F)
            {
                _fill.gameObject.SetActive(true);
                _background.gameObject.SetActive(true);
            }
            else
            {
                _fill.gameObject.SetActive(false);
                _background.gameObject.SetActive(false);
            }
        }

        private void SetFill(Stats stats)
        {
            _fill.fillAmount = stats.CurrentHP / stats.MaxHP;
            _timeSinceHealthChange = 0F;
        }

        private IEnumerator DisableBar()
        {
            yield return null;
            _timeSinceHealthChange = 5F;
        }
    }
}