using System;
using System.Linq;
using Assets.Scripts.Slime.Sugar;
using ConsoleApplication1;

namespace ClassLibrary1.Inventory
{
    /// <summary>
    /// Inventory with infinite cells. Limited by weight
    /// </summary>
    public class InfiniteInventory : AInventory<InventoryCell>, IInventory
    {
        public readonly bool SupportsGaps = false;
        public readonly bool SupportsEndlessItems = true;//TODO;

        public InfiniteInventory(float maxWeight, bool supportsGaps = false)
        {
            MaxWeight = maxWeight;
            SupportsGaps = supportsGaps;
        }

        protected override void Remove(InventoryCell cell, ref int index, ref int toRemove)
        {
            Program.Assert(toRemove > 0, "Trying to add less than zero");
            var removed = Math.Min(cell.ItemStack.Count, toRemove);

            var ii = cell.ItemStack;
            ItemStack.Accessor.AddCount(ii, -removed);
            toRemove -= removed;
            if (cell.ItemStack.Count == 0)
            {
                if (SupportsGaps)
                {
                    Items[index] = null;
                }
                else
                {
                    Items.RemoveAt(index);
                    index--;
                }
                InvokeItemRemoved(cell.ItemStack, removed);    
            }
            else
            {
                InvokeItemCountChanged(cell.ItemStack, cell.ItemStack.Count+removed, cell.ItemStack.Count);
            }
        }

        public int Add(ItemStack itemStack, int index, bool exactSlotOnly)
        {
            BaseAdd(itemStack, index, out var left, exactSlotOnly);

            if (!exactSlotOnly)
            {
                if (left > 0)
                {
                    if (index > Items.Count)
                    {
                        Items.AddMultipleTimes(null, index - Items.Count);
                    }
                }

                while (left > 0)
                {
                    var clone = itemStack.Clone();
                    var ii = new InventoryCell();
                    ii.ItemStack = clone;
                
                    if (index != -1)
                    {
                        Items.Insert(index, ii);
                        for (int x = index+1; x < Items.Count; x++)
                        {
                            var moved = Items[x];
                            if (moved != null) InvokeItemMoved(moved.ItemStack, x, x-1);
                        }
                        InvokeItemAdded (clone, index);
                    }
                    else
                    {
                        Items.Add(ii);
                        InvokeItemAdded (clone, Items.Count-1);
                    }
                
                    Append(ii, ref left);
                }
            }
            

            return left;
        }

        protected override void EnsureCapacity(int index)
        {
            if (index <= Items.Count)
            {
                Items.AddMultipleTimes(null, index - Items.Count + 1);
            } 
        }
        
        public void Move(int from, int to)
        {
            var item1 = Items.GetAt(from);
            var item2 = Items.GetAt(to);
            
            if (item1 == null && item2 == null) 
                return;

            var max = Math.Max(from, to);
            if (max < Items.Count)
            {
                Items.AddMultipleTimes(null, max - Items.Count);
            }
            
            if (item1 != null) InvokeItemMoved(item1.ItemStack, from, to);
            if (item2 != null) InvokeItemMoved(item2.ItemStack, to, from);
        }

        /// <summary>
        /// Max element for `for` loop
        /// </summary>
        public int Count => Items.Count;
        
        /// <summary>
        /// Total amount of cells
        /// </summary>
        public int TotalCells => Items.Count;
        
        /// <summary>
        /// Amount of free cells;
        /// </summary>
        public int FreeCells => int.MaxValue;
        
        /// <summary>
        /// Amount of cells with items
        /// </summary>
        public int UsedCells => Items.Count(x => x!=null && x.ItemStack != null);
    }
}