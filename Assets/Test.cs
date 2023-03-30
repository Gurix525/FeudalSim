using Items;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Container chest = new(4);
        Item stone = Item.Create("Stone");
        Item stone2 = stone.Clone();
        Item wood = Item.Create("Wood");
        Item sword = Item.Create("Sword");
        Item sword2 = Item.Create("Axe");

        chest.Insert(stone);
        chest.Insert(stone2);
        chest.ExtractAt(1, 8);
        chest.Insert(wood);
        chest.Insert(sword);
        //chest.ExtractAt(2);
        chest.Insert(sword2);

        Debug.Log(chest);
    }
}