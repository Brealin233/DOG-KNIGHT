using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    public Text requireName;
    public Text progressNumber;

    void Awake()
    {
        requireName = GetComponent<Text>();
        progressNumber = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetupRequirement(string name,int amount,int currentAmount)
    {
        requireName.text = name;
        progressNumber.text = currentAmount.ToString() + " / " + amount.ToString();
    }

    public void SetRequirement(string name,bool isFinished)
    {
        if (isFinished)
        {
            requireName.text = name;
            progressNumber.text = "完成";
            requireName.color = Color.gray;
            progressNumber.color = Color.gray;
        }
    }
}
