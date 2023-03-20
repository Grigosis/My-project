using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClassLibrary1.Inventory
{
    [Serializable]
    public class DollItemSlot
    {
        public int Filter;
        public Vector2 Position;
    }
    
    [Serializable]
    public class DollSettings
    {
        public List<DollItemSlot> ItemSlotFilters = new List<DollItemSlot>();
    }
    
    public class Doll
    {
        public FixedInventory Equipped;
        public FixedInventory Bag = new FixedInventory(float.PositiveInfinity, 80);
        private InfiniteInventory transaction = new InfiniteInventory(float.PositiveInfinity);
        public DollSettings Settings;
        public void Init(DollSettings settings)
        {
            Settings = settings;
            Equipped = new FixedInventory(float.PositiveInfinity, settings.ItemSlotFilters.Count);
            for (var i = 0; i < settings.ItemSlotFilters.Count; i++)
            {
                var slot = settings.ItemSlotFilters[i];
                var ii = (DollInventoryCell)Equipped.GetAt(i);
                ii.GearTypeFilter = slot.Filter;
                Debug.LogWarning("Init doll slot:" + slot.Filter);
            }
        }

        /// <summary>
        /// Takes off equipped item
        /// </summary>
        /// <param name="what">Coordinates in Equipped</param>
        /// <param name="where">Coordinates in bag</param>
        /// <returns>If takeoff was successful</returns>
        public bool TakeOff(int what, int where)
        {
            Debug.LogWarning("TaleOff:" + what);
            var get = Equipped.GetAt(what);
            if (get == null || get.ItemStack == null)
            {
                Debug.LogWarning("GET:" + (get == null));
                return false;
            }

            var item = get.ItemStack.Clone();
            Equipped.RemoveAt(what, -1);
            Bag.Add(item, where, false);
            return true;
        }
        
        
        
        /// <summary>
        /// Equips item
        /// </summary>
        /// <param name="what">Bag Coordinates</param>
        /// <param name="where">Equipped item coordinates</param>
        /// <returns>If was successful equip of an item</returns>
        public bool Equip(int what, int where)
        {
            
            var itemHolder1 = Bag.GetAt(what);
            var itemHolder2 = Equipped.GetAt(where);
            Debug.Log($"Equip Bag:{what} {itemHolder1} => Doll: {where} {itemHolder2}");
            if (itemHolder1?.ItemStack == null)
            {
                return TakeOff(where, what);
            }
            

            var item1 = itemHolder1.ItemStack.Clone();
            Bag.RemoveAt(what, -1);
            
            ItemStack item2 = null;
            if (itemHolder2?.ItemStack != null)
            {
                item2 = itemHolder2.ItemStack.Clone();
                Equipped.RemoveAt(where, -1);  
            }
            
            var count = Equipped.Add(item1, where, true);
            if (count > 0)
            {
                return false;
            }

            if (item2 != null)
            {
                Bag.Add(item2, what, false);
            }

            return true;
        }
    }
}