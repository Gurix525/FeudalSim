using System.Collections;
using System.Collections.Generic;
using Misc;
using TMPro;
using UnityEngine;

namespace WorldUI
{
    [RequireComponent(typeof(AlwaysFaceCamera))]
    public class Popup : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Vector3 _velocity = Vector3.up;
        [SerializeField] private float _fadeTime = 1F;
        [SerializeField] private AnimationCurve _scale = AnimationCurve.Linear(0F, 1F, 1F, 0F);
        [SerializeField] private AnimationCurve _alpha = AnimationCurve.Linear(0F, 1F, 1F, 0F);

        private TextMeshPro _text;
        private float _elapsedTime;

        private static Dictionary<string, GameObject> _prefabs = new();
        private static Dictionary<string, Pool<Popup>> _pools = new();
        private static Transform _popupsParent;

        #endregion Fields

        #region Properties

        public TextMeshPro Text => _text;

        #endregion Properties

        #region Public

        public static Popup Spawn(string name, Vector3 position, string content)
        {
            LoadPrefab(name);
            SpawnPool(name);
            Popup popup = _pools[name].Pull();
            popup.name = name;
            popup.transform.position = position;
            popup.Text.text = content;
            popup.gameObject.SetActive(true);
            return popup;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }

        private void OnEnable()
        {
            _elapsedTime = 0F;
        }

        private void FixedUpdate()
        {
            transform.position += _velocity * Time.fixedDeltaTime;
            transform.localScale = Vector3.one * _scale.Evaluate(_elapsedTime);
            Color oldColor = _text.color;
            _text.color = new(oldColor.r, oldColor.g, oldColor.b, _alpha.Evaluate(_elapsedTime));
            _elapsedTime += Time.fixedDeltaTime;
            if (_elapsedTime >= _fadeTime)
                ReturnToPool(this);
        }

        #endregion Unity

        #region Private

        private static void LoadPrefab(string name)
        {
            if (!_prefabs.ContainsKey(name))
            {
                _prefabs[name] = Resources.Load<GameObject>("Prefabs/WorldUI/" + name);
            }
        }

        private static void SpawnPool(string name)
        {
            if (!_pools.ContainsKey(name))
            {
                _pools[name] = new(_prefabs[name], _popupsParent ??= new GameObject("Popups").transform);
            }
        }

        private static void ReturnToPool(Popup popup)
        {
            _pools[popup.name].Push(popup);
        }

        #endregion Private
    }
}