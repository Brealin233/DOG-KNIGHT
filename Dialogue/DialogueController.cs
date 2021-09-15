using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueDate_SO currentDate;
    bool canTalk = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentDate != null)
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            canTalk = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && canTalk == true)
        {
            OpenDialogue();
            canTalk = false;
        }
    }

    void OpenDialogue()
    {
        DialogueUI.Instance.UpdateDialogueDate(currentDate);
        DialogueUI.Instance.UpdateMainDialogue(currentDate.dialoguePieces[0]);
    }
}
