using ClassLibrary1.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Slime.GUI
{
    public class InventoryItemPresentation : MonoBehaviour
    {
        public Image Image;
        public ItemStack ItemStack;

        public void SetItem(ItemStack stack)
        {
            Image.sprite = stack.Definition.Icon;
            ItemStack = stack;
        }
    }
}