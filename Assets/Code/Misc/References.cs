using System.Linq;
using UnityEngine;

namespace Misc
{
    public class References : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject[] _references;

        #endregion Fields

        #region Properties

        private static References Instance { get; set; }

        #endregion Properties

        #region Public

        public static GameObject GetReference(string name)
        {
            return Instance._references.ToList().Find(prefab => prefab.name == name);
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