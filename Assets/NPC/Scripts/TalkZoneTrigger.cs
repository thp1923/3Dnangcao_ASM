using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkZoneTrigger : MonoBehaviour
{
    public GameObject pressFPanel;
    public DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;

    private bool isPlayerNear = false;

    void Start()
    {
        pressFPanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            if (!dialogueManager.IsDialogueActive()) 
            {
                pressFPanel.SetActive(false);
                dialogueTrigger.StartDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            pressFPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            pressFPanel.SetActive(false);
        }
    }
}
