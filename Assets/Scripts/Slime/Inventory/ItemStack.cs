using System;
using Assets.Scripts.Slime.Sugar;
using ROR.Core.Serialization;

namespace ClassLibrary1.Inventory
{
    public class ItemStack
    {
        /// <summary>
        /// Should be used only by Inventory.
        /// </summary>
        public static class Accessor
        {
            public static void AddCount(ItemStack stack, int count)
            {
                stack.Count += count;
            }
            
            public static void SetCount(ItemStack stack, int count)
            {
                stack.Count = count;
            }
            
            public static void SetSettings(ItemStack stack, DynamicSettings settings)
            {
                stack.Settings = settings;
            }
            
            public static void SetItemDefinition(ItemStack stack, ItemDefinition definition)
            {
                stack.Definition = definition;
            }
            
            public static void SetCell(ItemStack stack, InventoryCell cell)
            {
                stack.Cell = cell;
            }
        }

        public ItemDefinition Definition { get; private set; }
        public DynamicSettings Settings { get; private set; } //Nullable
        public int Count { get; private set; }
            
        private InventoryCell m_cell;
        internal InventoryCell Cell
        {
            get {
                return m_cell;
            }
            set {
                if (m_cell != null && value != null)
                {
                    throw new Exception($"You must first free it from Cell [{m_cell}/{value}]");
                }

                m_cell = value;
            }
        }

        public ItemStack() { }

        public float Weight => Count * Definition.Weight;

        public ItemStack Clone()
        {
            var newItem = new ItemStack();
            newItem.Definition = Definition;
            newItem.Count = Count;
            newItem.Settings = Settings.Clone();
            return newItem;
        }

        public override bool Equals(object obj)
        {
            if (obj is ItemStack itemStack)
            {
                if (itemStack.Count != Count) return false;
                if (itemStack.Definition != Definition) return false;
                if (!Equals(itemStack.Settings,Settings)) return false;
                return true;
            }
            return false;
        }

        public ItemStack(ItemDefinition definition, DynamicSettings settings, int count)
        {
            Definition = definition;
            Settings = settings;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Definition?.Id ?? "NULL_ID"}/{Count}/{Settings?.ToString() ?? "NULL_SETTINGS"}";
        }
    }
}