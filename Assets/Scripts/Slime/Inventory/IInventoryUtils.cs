namespace ClassLibrary1.Inventory
{
    public interface IInventoryUtils
    {
        void Add(ItemDefinition definition, ref uint amount);
        void Remove(ItemDefinition definition, ref uint amount);
        uint Count(ItemDefinition definition);
    }
}