using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragDate
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }

    [Header("Inventory Date")]
    public InventoryDate_SO inventoryTemplate;
    public InventoryDate_SO inventoryDate;
    public InventoryDate_SO actionTemplate;
    public InventoryDate_SO actionDate;
    public InventoryDate_SO equipmentTemplate;
    public InventoryDate_SO equipmentDate;

    [Header("Container")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragDate currentDrag;

    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statsPanel;

    [Header("Stats Text")]
    public Text healthText;
    public Text attackText;
    public Text baseDefenceText;
    public Text currentDefenceText;
    public Text criticalText;

    [Header("Tooltip")]
    public ItemTooltip tooltip;

    bool isOpen = false;

    protected override void Awake()
    {
        base.Awake();

        if (inventoryTemplate != null)
            inventoryDate = Instantiate(inventoryTemplate);
        if (actionTemplate != null)
            actionDate = Instantiate(actionTemplate);
        if (equipmentTemplate != null)
            equipmentDate = Instantiate(equipmentTemplate);
    }

    private void Start()
    {
        LoadDate();
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
        }

        UpdateStateText(GameManager.Instance.playStats.CurrentHealth, GameManager.Instance.playStats.attackDate.minDamge, GameManager.Instance.playStats.attackDate.maxDamge
            ,GameManager.Instance.playStats.BaseDenfense,GameManager.Instance.playStats.CurrentDenfense,GameManager.Instance.playStats.attackDate.criticalChance.ToString());
    }

    public void SaveDate()
    {
        SaveManager.Instance.Save(inventoryDate, inventoryDate.name);
        SaveManager.Instance.Save(actionDate, actionDate.name);
        SaveManager.Instance.Save(equipmentDate, equipmentDate.name);
    }

    public void LoadDate()
    {
        SaveManager.Instance.Load(inventoryDate, inventoryDate.name);
        SaveManager.Instance.Load(actionDate, actionDate.name);
        SaveManager.Instance.Load(equipmentDate, equipmentDate.name);
    }

    public void UpdateStateText(int health,int min,int max,int defence,int curDenfence,string critical)
    {
        healthText.text = health.ToString();
        attackText.text = min + " ~ " + max;
        baseDefenceText.text = defence + " ~ " + curDenfence.ToString();
        criticalText.text = critical;
    } 

    #region 检查物品是否在slot内
    public bool CheckInInventoryUI(Vector3 postion)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, postion)) //括号内是一个方法,判断是否在格子内,返回一个bool值
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInActionUI(Vector3 postion)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, postion)) //括号内是一个方法,判断是否在格子内,返回一个bool值
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInequipmentUI(Vector3 postion)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, postion)) //括号内是一个方法,判断是否在格子内,返回一个bool值
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region 检测任务物品

    public void CheckQuestItemInBag(string questItemName)
    {
        foreach (var item in inventoryDate.items)
        {
            if(item.itemDate != null)
            {
                if(item.itemDate.itemName == questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemDate.itemName, item.amount);
                }
            }
        }

        foreach (var item in actionDate.items)
        {
            if (item.itemDate != null)
            {
                if (item.itemDate.itemName == questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemDate.itemName, item.amount);
                }
            }
        }
    }

    #endregion

    public InventoryItem QuestItemInBag(ItemDate_SO questItem)
    {
        return inventoryDate.items.Find(i => i.itemDate == questItem);
    }

    public InventoryItem QuestItemInAction(ItemDate_SO questItem)
    {
        return actionDate.items.Find(i => i.itemDate == questItem);
    }
}
