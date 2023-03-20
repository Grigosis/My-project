using System;
using System.Linq;
using Assets.Scripts.Slime.Sugar;
using ConsoleApplication1;
using UnityEngine;


namespace ClassLibrary1.Inventory
{
    public class DollInventoryCell : InventoryCell
    {
        public int GearTypeFilter;

        public DollInventoryCell()
        {
            CanStore = CanStoreImpl;
        }

        private bool CanStoreImpl(ItemStack x)
        {
            Debug.Log($"CanStoreImpl: {x.Definition.GearFlags} / {GearTypeFilter}");
            if ((x.Definition.GearFlags & GearTypeFilter) != 0)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"DollInventoryCell:{(ItemStack == null ? "null" : ItemStack.ToString())} {GearTypeFilter}";
        }
    }
    
    public class FixedInventory : AInventory<DollInventoryCell>, IInventory
    {
        public FixedInventory(float maxWeight, int cellCount)
        {
            MaxWeight = maxWeight;
            for (int i = 0; i < cellCount; i++)
            {
                Items.Add(new DollInventoryCell());    
            }
        }

        public void Extend(int cellCount)
        {
            for (int i = 0; i < cellCount; i++)
            {
                Items.Add(new DollInventoryCell());    
            }
        }

        public int Add(ItemStack itemStack, int index, bool exactSlotOnly)
        {
            itemStack = itemStack.Clone();
            BaseAdd(itemStack, index, out var left, exactSlotOnly);

            if (!exactSlotOnly)
            {
                index = index == -1 ? 0 : index;
                for (int i = 0; i < Items.Count && left > 0; i++)
                {
                    var index2 = ((i + index) % Items.Count);
                    RawAdd(itemStack, index2, ref left);
                }
            }
            

            return left;
        }

        public void Move(int from, int to)
        {
            if (Math.Max(to,from) >= Items.Count) 
                return;

            var item1 = Items.GetAt(from);
            var item2 = Items.GetAt(to);

            if (item1.ItemStack == null && item2.ItemStack == null)
            {
                return;
            }

            if (!item1.CanStore(item2.ItemStack) || !item2.CanStore(item1.ItemStack))
            {
                return;
            }

            var a = item1.ItemStack;
            var b = item2.ItemStack;

            item1.ItemStack = b;
            item2.ItemStack = a;

            if (a != null) InvokeItemMoved(item1.ItemStack, from, to);
            if (b != null) InvokeItemMoved(item2.ItemStack, to, from);
        }

        protected override void EnsureCapacity(int index)
        {
            if (index < Items.Count)
            {
                throw new Exception("Cant add null cells");
            } 
        }

        protected override void Remove(InventoryCell cell, ref int index, ref int toRemove)
        {
            Program.Assert(toRemove > 0, "Trying to add less than zero");
            var itemStack = cell.ItemStack;
            var removed = Math.Min(itemStack.Count, toRemove);
            ItemStack.Accessor.SetCount(itemStack, -removed);
            toRemove -= removed;
            if (itemStack.Count == 0)
            {
                cell.ItemStack = null;
                InvokeItemRemoved(itemStack, removed);
                InvokeItemCountChanged(itemStack, itemStack.Count+removed, itemStack.Count);
            }
            else
            {
                InvokeItemCountChanged(itemStack, itemStack.Count+removed, itemStack.Count);
            }
        }

        public int Count => Items.Count;
        public int TotalCells => Items.Count;
        public int FreeCells => Items.Count - UsedCells;
        public int UsedCells => Items.Count((x)=> x.ItemStack != null);
    }
}