using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    DialogueController controller;
    QuestDate_SO currentQuest;

    public DialogueDate_SO startDialogue;
    public DialogueDate_SO progressDialogue;
    public DialogueDate_SO completeDialogue;
    public DialogueDate_SO finishedDialogue;

    #region 获得任务状态
    public bool isStarted
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).isStarted;
            }
            else return false; 
        }
    }

    public bool isComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).isComplete;
            }
            else return false;
        }
    }

    public bool isFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).isFinished;
            }
            else return false;
        }
    }
    #endregion

    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }

    void Start()
    {
        controller.currentDate = startDialogue;
        currentQuest = controller.currentDate.GetQuest();
    }

    private void Update()
    {
        if (isStarted)
        {
            if (isComplete)
            {
                controller.currentDate = completeDialogue;
            }
            else
            {
                controller.currentDate = progressDialogue;
            }
        }

        if (isFinished)
            controller.currentDate = finishedDialogue;
    }
}
