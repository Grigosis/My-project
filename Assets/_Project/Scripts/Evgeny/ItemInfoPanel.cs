using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SecondCycleGame
{
    public class ItemInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private List<TextMeshProUGUI> _specificationsName = new List<TextMeshProUGUI>();
        [SerializeField] private List<TextMeshProUGUI> _specificationsValue;
        [SerializeField] private Button _okButton;

        public void Init(IItem item) {
            GetSpecification(item);
            _icon.sprite = item.Icon;
            _name.text = item.Name;
            _okButton.onClick.AddListener(OkClick);
        }

        private void OkClick() {
            gameObject.SetActive(false);
        }

        private void GetSpecification(IItem item) {
            switch (item.Type) {
                case ItemType.Armor: IArmorItem armorItem = (IArmorItem)item; armorItem.GetSpecification(ref _specificationsName, ref _specificationsValue); break;
                case ItemType.MeleeWeapons: IMeleWeaponItem meleItem = (IMeleWeaponItem)item; meleItem.GetSpecification(ref _specificationsName, ref _specificationsValue); break;
                case ItemType.RangedWeapons: IRangedWeaponsItem rangeItem = (IRangedWeaponsItem)item; rangeItem.GetSpecification(ref _specificationsName, ref _specificationsValue); break;
            }
        }
    }
}
