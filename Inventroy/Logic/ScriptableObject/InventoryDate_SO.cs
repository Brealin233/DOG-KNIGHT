using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory Date",fileName = "New Inventory")]
public class InventoryDate_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemDate_SO NewItemDate,int amount)
    {
        bool found = false;

        if (NewItemDate.stackable)
        {
            foreach (var item in items)
            {
                if (item.itemDate == NewItemDate)
                {
                    //item.itemDate = NewItemDate;
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
        }


        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].itemDate == null && !found)
            {
                items[i].itemDate = NewItemDate;
                items[i].amount = amount;
                break;
            }
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemDate_SO itemDate;
    public int amount;
}
