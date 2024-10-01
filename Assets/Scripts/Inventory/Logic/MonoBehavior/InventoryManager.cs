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
    public InventoryData_SO inventoryTemplate;
    public InventoryData_SO inventoryData;

    public InventoryData_SO actionTemplate;
    public InventoryData_SO actionData;

    public InventoryData_SO equipmentTemplate;
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
    public Text DefenceText;

    [Header("Tooltip")]
    public ItemTooptip itemTooltip;

    bool isOpen = false;

    protected override void Awake()
    {
        base.Awake();
        if (inventoryTemplate != null)
            inventoryData = Instantiate(inventoryTemplate);
        if (actionTemplate != null)
            actionData = Instantiate(actionTemplate);
        if (equipmentTemplate != null)
            equipmentData = Instantiate(equipmentTemplate);
    }

    void Start()
    {
        LoadData();
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
            GameManager.Instance.playerStatus.attackData.maxDamage,
            GameManager.Instance.playerStatus.currentDefence);
    }

    public void SaveData()
    {
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);
    }

    public void LoadData()
    {
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);
    }

    public void UpdateStatusText(int health, int min, int max, int defence)
    {
        healthText.text = health.ToString("00");
        attaclText.text = min + " - " + max;
        DefenceText.text = defence.ToString("00");
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

    #region Check if the quest item already exists in the inventory, if so, update the quest progress
    public void CheckQuestItemInBag(string questItemName)
    {
        foreach (var item in inventoryData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                    QuestManager.Instance.UpdateQuestProgress(questItemName, item.amount);
            }
        }

        foreach (var item in actionData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                    QuestManager.Instance.UpdateQuestProgress(questItemName, item.amount);
            }
        }
    }
    #endregion
}
