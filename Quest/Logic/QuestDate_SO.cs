using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest",menuName ="Quest/Quest Date")]
public class QuestDate_SO : ScriptableObject
{
    [System.Serializable]
    public class QuesrRequire
    {
        public string name;
        public int requireAmount;
        public int currentAmount;
    }

    public string questName;
    [TextArea]
    public string description;
    public bool isStarted;
    public bool isComplete;
    public bool isFinished;

    public List<QuesrRequire> questRequires = new List<QuesrRequire>();
    public List<InventoryItem> rewards = new List<InventoryItem>();

    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount); //r = Questrequire
        isComplete = finishRequires.Count() == questRequires.Count;
    }

    public void GiveRewards()
    {
        foreach (var reward in rewards)
        {
            if(reward.amount < 0)
            {
                int requireCount = Mathf.Abs(reward.amount);

                if(InventoryManager.Instance.QuestItemInBag(reward.itemDate) != null)
                {
                    if(InventoryManager.Instance.QuestItemInBag(reward.itemDate).amount <= requireCount)
                    {
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemDate).amount;
                        InventoryManager.Instance.QuestItemInBag(reward.itemDate).amount = 0;

                        if (InventoryManager.Instance.QuestItemInAction(reward.itemDate) != null)
                                InventoryManager.Instance.QuestItemInAction(reward.itemDate).amount -= requireCount;
                    }
                    else
                    {
                        InventoryManager.Instance.QuestItemInBag(reward.itemDate).amount -= requireCount;
                    }
                }
                else
                {
                    InventoryManager.Instance.QuestItemInAction(reward.itemDate).amount -= requireCount;
                }
            }
            else
            {
                InventoryManager.Instance.inventoryDate.AddItem(reward.itemDate, reward.amount);
            }
        }

        InventoryManager.Instance.inventoryUI.RefreshUI();
        InventoryManager.Instance.actionUI.RefreshUI();
    }

    public List<string> RequireTargetName()
    {
        List<string> targetNameList = new List<string>();

        foreach (var require in questRequires)
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }
}
