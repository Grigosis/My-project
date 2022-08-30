using System;
using ROR.Core.Serialization;

namespace ClassLibrary1.Inventory
{
    public interface IInventory
    {
        /// <summary>
        /// Adds item stack to inventory
        /// </summary>
        /// <param name="itemStack">items to add (with amount)</param>
        /// <param name="index">-1 - default behavior, other values - would try to add to this location</param>
        /// <returns>amount of items, that wasn't added</returns>
        int Add(ItemStack itemStack, int index = -1, bool exactSlotOnly = false);
        
        /// <summary>
        /// Removes items by definition.
        /// </summary>
        /// <param name="definition">Removed items should have save definition</param>
        /// <param name="amount">amount of items to remove</param>
        /// <returns>Amount of items that wasn't removed</returns>
        int Remove(ItemDefinition definition, int amount);
        
        /// <summary>
        /// Use -1 for complete remove item
        /// </summary>
        /// <param name="index">Index of item to remove</param>
        /// <param name="amount">Use -1 to remove all</param>
        /// <returns>Amount of items that wasn't removed</returns>
        int RemoveAt(int index, int amount = -1);
        
        
        /// <summary>
        /// Gets inventory cell at exact position
        /// </summary>
        /// <param name="index">Position</param>
        /// <returns></returns>
        InventoryCell GetAt(int index);
        ItemStack GetItemAt(int index);
        void Move(int from, int to);
        void Foreach(Action<ItemStack, int> action);
        bool CanAdd(ItemStack itemStack);
        bool CanRemove(ItemStack itemStack);

        
        event Action<ItemStack, int> OnItemAdded;
        event Action<ItemStack, int> OnItemRemoved;
        event Action<ItemStack, int, int> OnItemMoved;
        event Action<ItemStack, int, int> OnItemCountChanged;
        event Action<float, float> OnWeightChanged;
        
        Func<ItemStack, int> StackAmount { get; set; }
        Func<ItemStack, int, float> StackWeight { get; set; }
        

        int Count { get; }
        int TotalCells { get; }
        int FreeCells { get; }
        int UsedCells { get; }

        
    }
}