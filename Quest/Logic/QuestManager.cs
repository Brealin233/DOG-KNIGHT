using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask
    {
        public QuestDate_SO questDate;
        public bool isStarted { get { return questDate.isStarted; } set { questDate.isStarted = value; } }
        public bool isComplete { get { return questDate.isComplete; } set { questDate.isComplete = value; } }
        public bool isFinished { get { return questDate.isFinished; } set { questDate.isFinished = value; } }
    }

    public List<QuestTask> tasks = new List<QuestTask>();

    void Start()
    {
        LoadQuestManager();
    }

    public void LoadQuestManager()
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");

        for (int i = 0; i < questCount; i++)
        {
            var newQuest = ScriptableObject.CreateInstance<QuestDate_SO>();
            SaveManager.Instance.Load(newQuest, "task" + i);
            tasks.Add(new QuestTask { questDate = newQuest });
        }
    }

    public void SaveQuestManager()
    {
        PlayerPrefs.SetInt("QuestCount", tasks.Count);
        for (int i = 0; i < tasks.Count; i++)
        {
            SaveManager.Instance.Save(tasks[i].questDate, "task" + i);
        }
    }

    public void UpdateQuestProgress(string requireName,int amount)
    {
        foreach (var task in tasks)
        {
            if (task.isFinished)
                continue;
            var matchTask = task.questDate.questRequires.Find(q => q.name == requireName);
            if (matchTask != null)
                matchTask.currentAmount += amount;

            task.questDate.CheckQuestProgress();
        }
    }

    public bool HaveQuest(QuestDate_SO date)
    {
        if (date != null)
        {
            return tasks.Any(q => q.questDate.questName == date.questName);
        }
        else return false;
    }

    public QuestTask GetTask(QuestDate_SO date)
    {
        return tasks.Find(q => q.questDate.questName == date.questName);
    }
}
