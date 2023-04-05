using System.Linq;
using UnityEngine;

namespace Misc
{
    public class Meshes : MonoBehaviour
    {
        [SerializeField] private Mesh[] _meshes;

        private static Meshes _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static Mesh GetMesh(string name)
        {
            return _instance._meshes.ToList().Find(sprite => sprite.name == name);
        }
    }
}