using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;
    public ItemTooltip tooltip;
    bool isOpen = false;

    [Header("Quest Name")]
    public RectTransform questListTransform;
    public QuestNameButton questNameButton;

    [Header("Text Content")]
    public Text questContentText;

    [Header("Requirement")]
    public RectTransform requireTransform;
    public QuestRequirement requirement;

    [Header("Reward Panel")]
    public RectTransform rewardTransform;
    public ItemUI rewardUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            questContentText.text = string.Empty;

            SetQuestList();

            if (!isOpen)
                tooltip.gameObject.SetActive(false);
        }
    }

    private void SetQuestList()
    {
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, questListTransform);
            newTask.SetUpNameButton(item.questDate);
            newTask.questContentText = questContentText;
        }
    }

    public void SetupRequireList(QuestDate_SO questDate)
    {
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in questDate.questRequires)
        {
            var q = Instantiate(requirement, requireTransform);
            if (questDate.isFinished)
                q.SetRequirement(item.name, true);
            else
                q.SetupRequirement(item.name, item.requireAmount, item.currentAmount);
        }
    }

    public void SetupRewardItem(ItemDate_SO itemDate,int amount)
    {
        var item = Instantiate(rewardUI, rewardTransform);
        item.SetUpItemUI(itemDate, amount);
    }
}
