using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace VFX
{
    public class Effect : MonoBehaviour
    {
        #region Fields

        protected VisualEffect _visualEffect;

        private static Dictionary<string, GameObject> _prefabs = new();
        private static Dictionary<string, Pool<Effect>> _pools = new();
        private static Transform _vfxParent;

        #endregion Fields

        #region Properties

        public UnityEvent<Effect> EffectFinished { get; } = new();

        public VisualEffect VisualEffect => _visualEffect;

        #endregion Properties

        #region Public

        public static Effect Spawn(string name, Vector3 position)
        {
            LoadPrefab(name);
            SpawnPool(name);
            Effect effect = _pools[name].Pull();
            effect.EffectFinished.AddListener(ReturnToPool);
            effect.name = name;
            effect.transform.position = position;
            effect.gameObject.SetActive(true);
            return effect;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _visualEffect = transform.GetChild(0).GetComponent<VisualEffect>();
        }

        private void OnEnable()
        {
            StartCoroutine(FadeOff());
        }

        #endregion Unity

        #region Private

        private IEnumerator FadeOff()
        {
            while (true)
            {
                yield return null;
                if (_visualEffect.HasAnySystemAwake())
                    continue;
                EffectFinished.Invoke(this);
                yield break;
            }
        }

        private static void LoadPrefab(string name)
        {
            if (!_prefabs.ContainsKey(name))
            {
                _prefabs[name] = Resources.Load<GameObject>("Prefabs/VFX/" + name);
            }
        }

        private static void SpawnPool(string name)
        {
            if (!_pools.ContainsKey(name))
            {
                _pools[name] = new(_prefabs[name], _vfxParent ??= new GameObject("VFX").transform);
            }
        }

        private static void ReturnToPool(Effect effect)
        {
            effect.EffectFinished.RemoveListener(ReturnToPool);
            _pools[effect.name].Push(effect);
        }

        #endregion Private
    }
}