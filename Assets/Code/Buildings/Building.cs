using Items;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public Recipe Recipe { get; set; }
        [field: SerializeField] public Sprite RenderSprite { get; set; }

        public static GameObject[] Structures { get; private set; }
        public static GameObject[] Furniture { get; private set; }

        public static void LoadResources()
        {
            Structures = Resources.LoadAll<GameObject>("Prefabs/Buildings/Structures");
            Furniture = Resources.LoadAll<GameObject>("Prefabs/Buildings/Furniture");
            foreach (GameObject structure in Structures)
                structure.GetComponent<Building>().RenderSprite = PNGScanner.RenderSprite(structure);
        }
    }
}