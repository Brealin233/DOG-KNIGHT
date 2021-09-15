using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType { BAG,WEAPON,ARMOR,ACTION}
public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    public float buffTime = 5f;
    public bool buffTrigger = false;
    public bool isComplete = false;

    void Update()
    {
        if(buffTrigger == true)
        {
            isComplete = false;
            buffTime -= Time.deltaTime;
        }

        if(buffTime <= 0)
        {
            isComplete = true;
            buffTrigger = false;
            buffTime = 5f;
        }

       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem() != null)
        {
            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                GameManager.Instance.playStats.ApplyHealth(itemUI.GetItem().useableDate.healthPoint);

                GameManager.Instance.playStats.ApplyBuff(itemUI.GetItem().useableDate.buffAttack, itemUI.GetItem().useableDate.buffCriticalChance);
                buffTrigger = true;


                itemUI.Bag.items[itemUI.Index].amount -= 1;
                QuestManager.Instance.UpdateQuestProgress(itemUI.GetItem().itemName, -1);
            }
            UpdateItem();
        }
    }

    public void UpdateItem()
    {
        switch(slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryDate;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentDate;
                if(itemUI.Bag.items[itemUI.Index].itemDate != null)
                {
                    GameManager.Instance.playStats.ChangeWeapon(itemUI.Bag.items[itemUI.Index].itemDate);
                }
                else
                {
                    GameManager.Instance.playStats.UnEquipWeapon();
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentDate;
                if (itemUI.Bag.items[itemUI.Index].itemDate != null)
                {
                    GameManager.Instance.playStats.ChangeAromor(itemUI.Bag.items[itemUI.Index].itemDate);
                }
                else
                {
                    GameManager.Instance.playStats.UnEquipArmor();
                }
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionDate;
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetUpItemUI(item.itemDate, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.tooltip.SetTooltip(itemUI.GetItem());
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }
}
