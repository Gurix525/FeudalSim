using Items;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Container chest = new(4);
        Item stone = Item.Create("Stone", 7);
        Item stone2 = Item.Create("Stone", 6);
        Item wood = Item.Create("Wood");
        Item sword = Item.Create("Sword");
        Item sword2 = Item.Create("Axe");

        chest.InsertAt(0, stone);
        chest.InsertAt(3, stone2);
        chest.InsertAt(2, wood);
        //chest.Insert(stone);
        //chest.Insert(stone2);
        //chest.ExtractAt(1, 8);
        //chest.Insert(wood);
        //chest.Insert(sword);
        //chest.Insert(sword2);

        Debug.Log(chest);
        chest.Sort();
        Debug.Log(chest);
    }
}