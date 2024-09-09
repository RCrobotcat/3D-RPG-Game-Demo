using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType { BAG, WEAPON, ARMOR, ACTION }
public class SlotHolder : MonoBehaviour, IPointerClickHandler
{
    public ItemUI itemUI;
    public SlotType slotType;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem().itemType == ItemType.Usable && itemUI.Bag.items[itemUI.Index].amount > 0)
        {
            GameManager.Instance.playerStatus.ApplyHealth(itemUI.GetItem().usableItemData.RestoreHealthPoint);
            itemUI.Bag.items[itemUI.Index].amount--; // decrease the amount by 1
        }
        UpdateItem();
    }

    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                if (itemUI.Bag.items[itemUI.Index].itemData != null)
                    GameManager.Instance.playerStatus.SwitchWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                else
                    GameManager.Instance.playerStatus.UnEquipWeapon();
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetUpItemUI(item.itemData, item.amount);
    }
}
