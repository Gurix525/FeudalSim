using System.Linq;
using UnityEngine;

namespace Misc
{
    public class Materials : MonoBehaviour
    {
        [SerializeField] private Material[] _materials;

        private static Materials Instance;

        public static Material DefaultMaterial => Instance._materials[0];

        private void Awake()
        {
            Instance = this;
        }

        public static Material GetMaterial(string name)
        {
            return Instance._materials.ToList().Find(sprite => sprite.name == name);
        }
    }
}