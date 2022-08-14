using ClassLibrary1.Inventory;
using UnityEngine;

namespace Assets.Scripts.Slime.Inventory
{
    public class InventoryPrinter
    {
        private string Name;
        public InventoryPrinter (IInventory inventory, string name)
        {
            Name = name;
            inventory.OnItemAdded += OnItemAdded;
            inventory.OnItemMoved += OnItemMoved;
            inventory.OnItemRemoved += OnItemRemoved;
            inventory.OnWeightChanged += OnWeightChanged;
            inventory.OnItemCountChanged += OnItemCountChanged;
            
            Debug.Log($"{Name} {inventory}");
        }
        
        private void OnItemCountChanged(ItemStack arg1, int arg2, int arg3)
        {
            Debug.Log($"{Name} OnItemCountChanged {arg2} => {arg3}");
        }

        private void OnWeightChanged(float arg1, float arg2)
        {
            Debug.Log($"{Name} OnWeightChanged {arg1} => {arg2}");
        }

        private void OnItemRemoved(ItemStack arg1, int arg2)
        {
            Debug.Log($"{Name} OnItemRemoved {arg1} => {arg2}");
        }

        private void OnItemMoved(ItemStack arg1, int arg2, int arg3)
        {
            Debug.Log($"{Name} OnItemMoved {arg2} => {arg3}");
        }

        private void OnItemAdded(ItemStack arg1, int arg2)
        {
            Debug.Log($"{Name} OnItemCountChanged {arg1} => {arg2}");
        }
    }
}