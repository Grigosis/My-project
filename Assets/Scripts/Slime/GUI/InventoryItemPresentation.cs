using Assets.Scripts.Slime.Core;
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
            var icon = stack.Definition.Icon;
            Image.sprite = R.Instance.Load<Sprite>(icon);
            ItemStack = stack;
        }
    }
}