using UnityEngine;
using System.Linq;

namespace Misc
{
    public class Prefabs : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject[] _prefabs;

        #endregion Fields

        #region Properties

        private static Prefabs Instance { get; set; }

        #endregion Properties

        #region Public

        public static GameObject GetPrefab(string name)
        {
            return Instance._prefabs.ToList().Find(prefab => prefab.name == name);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            Instance = this;
        }

        #endregion Unity
    }
}