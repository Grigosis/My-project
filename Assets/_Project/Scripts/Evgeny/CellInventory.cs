using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SecondCycleGame
{
    public class CellInventory : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] ItemInfoPanel _itemInfoPanel;
        [SerializeField] private Image _iconImage;
        private IItem _currentItem;

        public void SetCurrentItem(IItem newItem) {
            _currentItem = newItem;
            _iconImage.sprite = _currentItem.Icon;
        }
        public void OpenInfoPanel() {
            _itemInfoPanel.gameObject.SetActive(true);
            _itemInfoPanel.Init(_currentItem);
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (Input.GetMouseButtonDown(1)) {
                _itemInfoPanel.gameObject.SetActive(true);
                _itemInfoPanel.Init(_currentItem);
            }
        }
    }
}
