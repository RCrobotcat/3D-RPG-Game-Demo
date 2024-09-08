using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public Text amount = null;

    public InventoryData_SO Bag { get; set; }
    public int Index { get; set; } = -1;

    public void SetUpItemUI(ItemData_SO item, int itemAmount)
    {
        if (item != null)
        {
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString("00"); // format the amount to 2 digits
            icon.gameObject.SetActive(true);
        }
        else icon.gameObject.SetActive(false);
    }
}
