using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuStarter : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _settingsCanvas;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.sortingOrder = 2000;
            StartCoroutine(Darken());
            StartCoroutine(EnableSettings());
        }

        private IEnumerator Darken()
        {
            yield return null;
            yield return null;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForFixedUpdate();
                _image.color = new(0F, 0F, 0F, 1F - (i / 100F));
            }
            _canvas.sortingOrder = -2000;
        }

        private IEnumerator EnableSettings()
        {
            yield return null;
            _settingsCanvas.SetActive(true);
            yield return null;
            yield return null;
            _settingsCanvas.SetActive(false);
        }
    }
}