using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;
    public QuestDate_SO currentDate;
    public Text questContentText;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent()
    {
        questContentText.text = currentDate.description;
        QuestUI.Instance.SetupRequireList(currentDate);

        foreach (Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in currentDate.rewards)
        {
            QuestUI.Instance.SetupRewardItem(item.itemDate, item.amount);
        }
    }

    public void SetUpNameButton(QuestDate_SO questDate)
    {
        currentDate = questDate;

        if (questDate.isComplete)
            questNameText.text = questDate.questName + "(完成)";
        else
            questNameText.text = questDate.questName;
    }
}
