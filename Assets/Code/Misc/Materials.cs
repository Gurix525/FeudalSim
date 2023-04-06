using System.Linq;
using UnityEngine;

namespace Misc
{
    public class Materials : MonoBehaviour
    {
        [SerializeField] private Material[] _materials;

        private static Materials _instance;

        public static Material DefaultMaterial => _instance._materials[0];

        private void Awake()
        {
            _instance = this;
        }

        public static Material GetMaterial(string name)
        {
            return _instance._materials.ToList().Find(sprite => sprite.name == name);
        }
    }
}