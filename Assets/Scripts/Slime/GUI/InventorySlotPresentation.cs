using System;
using ClassLibrary1.Inventory;
using UnityEngine;

namespace Assets.Scripts.Slime.GUI
{
    public class InventorySlotPresentation : MonoBehaviour
    {
        public int Index;
        public IInventory Inventory;
        public String DebugName;

        public InventoryCell GetCell()
        {
            return Inventory.GetAt(Index);
        }

        public InventoryItemPresentation GetItem()
        {
            return gameObject.GetComponentInChildren<InventoryItemPresentation>();
        }
        
        public ItemStack GetInventoryItem()
        {
            var i = GetItem();
            if (i == null) return null;
            return i.ItemStack;
        }
        
        public override string ToString()
        {
            return $"InventorySlotPresentation {DebugName}";
        }
    }
}