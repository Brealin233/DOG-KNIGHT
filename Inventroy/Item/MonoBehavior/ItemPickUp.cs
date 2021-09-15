using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemDate_SO itemDate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.inventoryDate.AddItem(itemDate, itemDate.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            QuestManager.Instance.UpdateQuestProgress(itemDate.itemName, itemDate.itemAmount);

            if (itemDate.armorDate != null)
            {
                if (itemDate.armorDate.isRandom == true)
                {
                    //itemDate.armorDate.compound += 1;
                    System.Random randomDefence = new System.Random();
                    int defence = randomDefence.Next(2, 8);

                    itemDate.armorDate.extraDefence = defence;

                    //if (itemDate.armorDate.compound >= 3)
                    //    itemDate.armorDate.compoundComplete = true;

                    Destroy(gameObject);
                }
            }

            Destroy(gameObject);

        }
    }
}
