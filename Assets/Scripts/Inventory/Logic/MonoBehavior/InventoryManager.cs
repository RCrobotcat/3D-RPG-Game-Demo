using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    // use to store the original holder and parent of the item being dragged
    // convienient to return the item to its original slot
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }

    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;

    [Header("Containers")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData currentDrag;

    [Header("UI Panels")]
    public GameObject BagPanel;
    public GameObject EquipmentPanel;

    [Header("Status Text")]
    public Text healthText;
    public Text attaclText;

    bool isOpen = false;

    void Start()
    {
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            BagPanel.SetActive(isOpen);
            EquipmentPanel.SetActive(isOpen);
        }

        UpdateStatusText(GameManager.Instance.playerStatus.currentHealth,
            GameManager.Instance.playerStatus.attackData.minDamage,
            GameManager.Instance.playerStatus.attackData.maxDamage);
    }

    public void UpdateStatusText(int health, int min, int max)
    {
        healthText.text = health.ToString("00");
        attaclText.text = min + " - " + max;
    }

    #region Judge the item being dragged is inside the range of the target slot
    public bool CheckInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slots.Length; i++)
        {
            // same as => (RectTransform) inventoryUI.slots[i].transform, typecasting
            RectTransform t = inventoryUI.slots[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                return true;
        }
        return false;
    }

    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slots.Length; i++)
        {
            // same as => (RectTransform) inventoryUI.slots[i].transform, typecasting
            RectTransform t = actionUI.slots[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                return true;
        }
        return false;
    }

    public bool CheckInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slots.Length; i++)
        {
            // same as => (RectTransform) inventoryUI.slots[i].transform, typecasting
            RectTransform t = equipmentUI.slots[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                return true;
        }
        return false;
    }
    #endregion
}
