using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Inventory;
using Assets.Scripts.Slime.Sugar;
using ClassLibrary1.Inventory;
using ROR.Core.Serialization;
using UnityEngine;

namespace Assets.Scripts.Slime.GUI
{
    
    public class InventoryManager : MonoBehaviour, IDraggableHandler
    {
        public GameObject ItemSlotPrefab;
        public GameObject ItemPrefab;
        public GameObject AttachParent;
        public GameObject DollAttachParent;
        public Transform DragParent;

        private List<InventorySlotPresentation> DollSlots = new List<InventorySlotPresentation>();
        private List<InventorySlotPresentation> InventorySlots = new List<InventorySlotPresentation>();
        
        public int SlotsInRow = 8;
        public int MinRows = 6;
        
        private int m_rows = 0;
        private Doll Doll = new Doll();

        public void StartDrag(DraggableReceiver old, Draggable draggable)
        {
            
        }
        
        public void EndDrag(DraggableReceiver old, DraggableReceiver neww, Draggable draggable)
        {
            
        }

        private void Update()
        {
            //CheckConsistency();
        }

        public void CheckConsistency()
        {
            for (int i = 0; i < Doll.Equipped.TotalCells; i++)
            {
                var item1 = Doll.Equipped.GetAt(i)?.ItemStack;
                var item2 = DollSlots[i].GetInventoryItem();
                if (!Equals(item1, item2))
                {
                    Debug.LogError($"UI Doll slot [{i}] has [{item2}], but Core has [{item1}]");
                }
            }
            
            for (int i = 0; i < Doll.Bag.TotalCells; i++)
            {
                var item1 = Doll.Bag.GetAt(i)?.ItemStack;
                var item2 = InventorySlots[i].GetInventoryItem();
                if (!Equals(item1, item2))
                {
                    Debug.LogError($"UI Bag slot [{i}] has [{item2}], but Core has [{item1}]");
                }
            }
        }
        
        public bool TryAttach(DraggableReceiver old, DraggableReceiver neww, Draggable draggable)
        {
            var oldSlot = old.GetComponent<InventorySlotPresentation>();
            var newSlot = neww.GetComponent<InventorySlotPresentation>();

            if (oldSlot == null || newSlot == null)
            {
                return true;
            }

            var oldDraggable = neww.Draggable;
            
            Debug.LogWarning($"TryAttach: {oldSlot} {draggable} | {newSlot} {oldDraggable}");
            
            if (oldSlot.Inventory == newSlot.Inventory && oldSlot.Inventory == Doll.Bag)
            {
                MoveItemGUI(old, neww, draggable, oldDraggable);
                Doll.Bag.Move(oldSlot.Index, newSlot.Index);
                return true;
            }
            else if (oldSlot.Inventory == newSlot.Inventory && oldSlot.Inventory == Doll.Equipped)
            {
                //TODO;
                return false;
            }
            else
            {
                if (newSlot.Inventory == Doll.Equipped)
                {
                    if (Doll.Equip(oldSlot.Index, newSlot.Index))
                    {
                        MoveItemGUI(old, neww, draggable, oldDraggable);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                } else if (newSlot.Inventory == Doll.Bag)
                {
                    if (Doll.TakeOff(oldSlot.Index, newSlot.Index))
                    {
                        MoveItemGUI(old, neww, draggable, oldDraggable);
                        return true;
                    }
                    else
                    {
                        Debug.Log("Can't take off");
                        return false;
                    }
                }
            }
            
            if (neww.Draggable != null)
            {
                
            }
            else
            {
                draggable.OnDropTo(neww);
            }
                //return false;
            
            return true;
        }

        private void MoveItemGUI(DraggableReceiver old, DraggableReceiver neww, Draggable draggable, Draggable oldDraggable)
        {
            old.Detach(draggable);
            draggable.OnDropTo(neww);
            draggable.ChangeParent();
            neww.Attach(draggable);
            
            if (oldDraggable != null)
            {
                neww.Detach(oldDraggable);
                oldDraggable.OnDropTo(old);
                oldDraggable.ChangeParent();
                old.Attach(oldDraggable);
            }
        }

        public DollSettings DollSettings = new DollSettings();
        
        public void Awake()
        {
            Doll.Init(DollSettings);
            Doll.Bag.Add(new ItemStack(D.Instance.Get<EquipmentDefinition>("Item/BaseWeapon"), null, 1), 0, true);
            Doll.Bag.Add(new ItemStack(D.Instance.Get<EquipmentDefinition>("Item/BaseArmor"), null, 1), 1, true);
            Doll.Bag.Add(new ItemStack(D.Instance.Get<EquipmentDefinition>("Item/BaseBoots"), null, 1), 2, true);
            Doll.Bag.Add(new ItemStack(D.Instance.Get<EquipmentDefinition>("Item/BaseGloves"), null, 1), 3, true);

            new InventoryPrinter(Doll.Bag, "Bag");
            new InventoryPrinter(Doll.Equipped, "Doll");
            
            Generate();
            GenerateDoll();
        }

        


        public void GenerateDoll()
        {
            for (var i = 0; i < DollSettings.ItemSlotFilters.Count; i++)
            {
                var setting = DollSettings.ItemSlotFilters[i];
                var cell = Doll.Equipped.GetAt(i);
                var itemSlot = CreateItemSlot(setting.Position.x, setting.Position.y, cell, DollAttachParent, Doll.Equipped, $"Doll slot {i} {((DollInventoryCell)cell).GearTypeFilter}");
                itemSlot.Index = i;
                DollSlots.Add(itemSlot);
            }
        }
        
        public void Generate()
        {
            var bagCount = Doll.Bag.TotalCells;
            var neededRows = bagCount.DivideWithUpperRound(SlotsInRow);
            neededRows = Math.Max(MinRows, neededRows);
            for (var i=0; i<neededRows; i++)
            {
                AddRow();
                m_rows++;
            }
        }

        
        public void AddRow()
        {
            for (var i=m_rows*SlotsInRow; i<(m_rows+1)*SlotsInRow; i++)
            {
                var x = i % SlotsInRow;
                var y = i / SlotsInRow;
                var cell = Doll.Bag.GetAt(i);
                var itemSlot = CreateItemSlot(x,y, cell, AttachParent, Doll.Bag, $"Slot {x}:{y}");
                itemSlot.Index = i;
                InventorySlots.Add(itemSlot);
            }
        }

        private InventorySlotPresentation CreateItemSlot(float x, float y, InventoryCell cell, GameObject AttachObject, IInventory inventory, String DebugName)
        {
            var o = Instantiate(ItemSlotPrefab, AttachObject.transform);
            var itemSlot = o.GetComponentInChildren<InventorySlotPresentation>();
            var receiver = o.GetComponentInChildren<DraggableReceiver>();
            var rectTransform = o.GetComponent<RectTransform>();
            itemSlot.DebugName = DebugName;
            itemSlot.Inventory = inventory;
            
            receiver.DebugName = DebugName;
            rectTransform.anchoredPosition = new Vector2(x * 100, -y * 100);

            if (cell != null && cell.ItemStack != null)
            {
                var item = Instantiate(ItemPrefab, AttachObject.transform);
                var itemPresentation = item.GetComponent<InventoryItemPresentation>();
                itemPresentation.SetItem(cell.ItemStack);
                var draggable = item.GetComponent<Draggable>();
                draggable.DragParent = DragParent;
                draggable.Canvas = GetComponentInParent<Canvas>();
                draggable.OnDropTo(receiver);
                draggable.ChangeParent();
            }

            return itemSlot;
        }
        
        
    }
}