using Controls;
using Items;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        private Item _backingItem;
        private BuildingMode _buildingMode;

        public void Initialize(Item item, BuildingMode buildingMode)
        {
            _backingItem = item;
            _buildingMode = buildingMode;
            GetComponent<MeshRenderer>().material.SetFloat("_Displacement", buildingMode switch
            {
                BuildingMode.Floor => 0F,
                BuildingMode.BigFloor => 0F,
                BuildingMode.ShortWall => 1F,
                BuildingMode.Wall => 1F,
                BuildingMode.BigWall => 1F,
                _ => 0F,
            });
        }
    }
}