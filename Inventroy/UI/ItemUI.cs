using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public Text amount = null;

    public ItemDate_SO currentItemDate;

    public InventoryDate_SO Bag { get; set; }
    public int Index { get; set; } = -1;

    public void SetUpItemUI(ItemDate_SO item,int itemAmount)
    {
        if(itemAmount == 0)
        {
            Bag.items[Index].itemDate = null;
            icon.gameObject.SetActive(false);
            return;
        }

        if (itemAmount < 0)
            item = null;

        if (item != null)
        {
            currentItemDate = item;
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false);
    }

    public ItemDate_SO GetItem()
    {
        return Bag.items[Index].itemDate;
    }
}
