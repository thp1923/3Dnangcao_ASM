using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<string> dialogueLines;

    public void StartDialogue()
    {
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        manager.StartDialogue(dialogueLines);
    }
}
