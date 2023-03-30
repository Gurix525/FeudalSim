using Items;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        ItemModel stoneModel = new("Stone", 10);
        ItemModel woodModel = new("Wood", 10);
        ItemModel swordModel = new("Sword1", 1);
        ItemModel swordModel2 = new("Sword2", 1);

        Container chest = new(4, string.Empty);
        Item stone = new(stoneModel, 10);
        Item stone2 = new(stoneModel, 10);
        Item wood = new(woodModel, 10);
        Item sword = new(swordModel, 1);
        Item sword2 = new(swordModel2, 1);

        chest.Insert(stone);
        chest.Insert(stone2);
        chest.ExtractAt(1, 8);
        chest.Insert(wood);
        chest.Insert(sword);
        chest.ExtractAt(2);
        chest.Insert(sword2);

        Debug.Log(chest);
    }
}