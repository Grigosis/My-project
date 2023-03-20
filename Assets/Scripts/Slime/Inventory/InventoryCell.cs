using System;

namespace ClassLibrary1.Inventory
{
    public class InventoryCell
    {
        private ItemStack m_itemStack;
        public ItemStack ItemStack {
            get
            {
                return m_itemStack;
            }
            set
            {
                if (m_itemStack != null && value != null)
                {
                    throw new Exception($"You need free ItemStack first [{m_itemStack}/{value}]");
                }

                m_itemStack = value;
                if (value != null)
                {
                    ItemStack.Accessor.SetCell(value, this);
                }
            }
        }
        public Func<ItemStack, bool> CanStore = (x)=>true;

        public override string ToString()
        {
            return $"InventoryCell:{(ItemStack == null ? "null" : ItemStack.ToString())}";
        }
    }
}