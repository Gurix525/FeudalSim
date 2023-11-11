using System.Linq;
using Items;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public Recipe Recipe { get; set; }
        [field: SerializeField] public Sprite RenderSprite { get; set; }
        [field: SerializeField] public Vector3 PivotOffset { get; set; }
        [field: SerializeField] public bool IsMultipleMode { get; set; }
        [field: SerializeField] public string Name { get; private set; }

        public static GameObject[] Structures { get; private set; }
        public static GameObject[] Furniture { get; private set; }

        public static void LoadResources()
        {
            Structures = Resources.LoadAll<GameObject>("Prefabs/Buildings/Structures");
            Furniture = Resources.LoadAll<GameObject>("Prefabs/Buildings/Furniture");
            foreach (GameObject structure in Structures.Concat(Furniture))
            {
                var building = structure.GetComponent<Building>();
                building.RenderSprite = PNGScanner.RenderSprite(structure);
                building.Name = structure.name;
            }
        }
    }
}