using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Slime.Sugar;
using ConsoleApplication1;
using ROR.Core.Serialization;
using UnityEngine;

namespace ClassLibrary1.Inventory
{
    public abstract class AInventory<T> where T : InventoryCell, new()
    {
        protected List<T> Items = new List<T>();
        public Func<ItemStack, int> StackAmount { get; set; } = (stack) => stack.Definition.StackSize;
        public Func<ItemStack, int, float> StackWeight { get; set; } = (stack, count) => (count * stack.Definition.Weight);
        
        
        private float m_externalWeight = 0;
        private float m_weight = 0;
        
        public float Weight
        {
            get=>m_weight;
            set
            {
                var was = m_weight;
                m_weight = value;
                OnWeightChanged?.Invoke(was, m_weight);
            }
        }

        public float ExternalWeight
        {
            get
            {
                return m_externalWeight;
            }
            set
            {
                var d = m_externalWeight - value;
                m_externalWeight = d;
                Weight += d;
            }
        }

        public float MaxWeight;

        public InventoryCell GetAt(int index)
        {
            return Items.GetAt(index);
        }
        
        public ItemStack GetItemAt(int index)
        {
            var cell = Items.GetAt(index);
            if (cell != null && cell.ItemStack != null)
            {
                return cell.ItemStack.Clone();
            }
            return null;
        }

        public int RemoveAt(int index, int amount)
        {
            var item = Items.GetAt(index);
            if (item != null && item.ItemStack != null)
            {
                amount = amount == -1 ? item.ItemStack.Count : amount; 
                Remove(item, ref index, ref amount);
                return amount;
            }

            return amount;
        }


        protected abstract void EnsureCapacity(int index);
        

        protected void RawAdd(ItemStack itemStack, int index, ref int left)
        {
            Program.Assert(left > 0, "Trying to add less than zero");
            
            var cell = Items.GetAt(index);
            if (cell == null)
            {
                cell = new T();
                EnsureCapacity(index);
                Items[index] = cell;
            }
            if (cell.CanStore(itemStack))
            {
                var canAdd = StackAmount(itemStack);
                if (canAdd > 0)
                {
                    cell.ItemStack = itemStack.Clone();
                    var added = Math.Min(left, canAdd);
                    ItemStack.Accessor.AddCount(cell.ItemStack, added);
                    left -= added;
                    Weight += StackWeight(cell.ItemStack, added);
                    InvokeItemAdded (cell.ItemStack, index);
                }
                else
                {
                    Debug.LogWarning($"Can add < 0: {StackAmount(itemStack)} {itemStack.Count}");
                }
            }
            else
            {
                Debug.Log($"Can't RawAdd {index} {left}");
            }
        }
        
        protected void BaseAdd(ItemStack itemStack, int index, out int left, bool exactSlotOnly = false)
        {
            left = itemStack.Count;
            ItemStack.Accessor.SetCount(itemStack, 0);

            AdjustFitAmount(itemStack, ref left);
            if (left <= 0)
            {
                return;
            }

            
            var ii = Items.GetAt(index);
            if (ii == null || (ii.ItemStack == null || (ii.ItemStack != null && ii.ItemStack.Definition == itemStack.Definition)) && ii.CanStore(itemStack))
            {
                if (ii?.ItemStack == null)
                {
                    RawAdd(itemStack, index, ref left);
                }
                else
                {
                    Append(ii, ref left);
                }
                
            }

            if (!exactSlotOnly)
            {
                var left2 = left;
                Foreach((stack, index2) =>
                {
                    if (left2 > 0 &&  stack != null && stack.Definition == itemStack.Definition && Items.GetAt(index2).CanStore(itemStack))
                    {
                        Append(GetAt(index2), ref left2);
                    }
                });
                left = left2;
            }
            
        }
        
        public int Remove(ItemDefinition definition, int amount)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var ii = Items[i];
                if (ii == null) continue;
                if (ii.ItemStack == null) continue;

                if (ii.ItemStack.Definition == definition)
                {
                    Remove(ii, ref i, ref amount);
                    if (amount == 0)
                    {
                        return amount;
                    }
                }
            }

            return amount;
        }

        protected abstract void Remove(InventoryCell cell, ref int index, ref int toRemove);

        protected void Append(InventoryCell cell, ref int toAdd)
        {
            Program.Assert(toAdd > 0, "Trying to add less than zero");
            var canAdd = StackAmount(cell.ItemStack) - cell.ItemStack.Count;
            if (canAdd > 0)
            {
                var added = Math.Min(toAdd, canAdd);
                ItemStack.Accessor.AddCount(cell.ItemStack, added);
                toAdd -= added;
                Weight += StackWeight(cell.ItemStack, added);
                InvokeItemCountChanged(cell.ItemStack, cell.ItemStack.Count-added, cell.ItemStack.Count);
            }
        }
        
        public void Foreach(Action<ItemStack, int> action)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                if (item != null && item.ItemStack != null)
                {
                    action.Invoke(item.ItemStack, i);
                }
            }
        }
        
        public Dictionary<ItemDefinition, int> GetAmounts()
        {
            var amounts = new Dictionary<ItemDefinition, int>();
            Foreach((stack, index) =>
            {
                amounts[stack.Definition] = amounts.GetOrNew(stack.Definition) + stack.Count;
            });
            return amounts;
        }

        public void AdjustFitAmount(ItemStack stack, ref int wantAdd)
        {
            var weight = (MaxWeight - Weight) / StackWeight(stack, wantAdd);
            var canAddWeight = weight > int.MaxValue ? int.MaxValue : (int)weight;
            wantAdd = Math.Min(wantAdd, canAddWeight);
        }
        
        public bool CanAdd(ItemStack itemStack)
        {
            if (itemStack.Weight + Weight > MaxWeight)
            {
                return false;   
            }

            return true;
        }

        public bool CanRemove(ItemStack itemStack)
        {
            return true;
        }


        protected void InvokeItemMoved(ItemStack stack, int from, int to)
        {
            OnItemMoved?.Invoke(stack, from, to);
        }
        
        protected void InvokeItemAdded(ItemStack stack, int index)
        {
            OnItemAdded?.Invoke(stack, index);
        }
        
        protected void InvokeItemRemoved(ItemStack stack, int index)
        {
            OnItemRemoved?.Invoke(stack, index);
        }
        
        protected void InvokeItemCountChanged(ItemStack stack, int from, int to)
        {
            OnItemCountChanged?.Invoke(stack, from, to);
        }
        
        
        public event Action<ItemStack, int> OnItemAdded;
        public event Action<ItemStack, int> OnItemRemoved;
        public event Action<ItemStack, int, int> OnItemMoved;
        public event Action<ItemStack, int, int> OnItemCountChanged;
        public event Action<float, float> OnWeightChanged;

        public override string ToString()
        {
            var sb = new StringBuilder();
            Foreach((a, i) =>
            {
                sb.Append($"{i}: ");
                if (a == null)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append(a.ToString());
                }
            });
            return sb.ToString();
        }
    }
}