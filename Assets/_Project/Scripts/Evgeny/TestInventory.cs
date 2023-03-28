using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public class TestInventory : MonoBehaviour
    {
        [SerializeField] private List<CellInventory> cells = new List<CellInventory>();

        void Start() {
            cells[0].SetCurrentItem(new Sword());
            cells[1].SetCurrentItem(new ClothArmor());
            cells[2].SetCurrentItem(new Pistol());
        }
    }
}
