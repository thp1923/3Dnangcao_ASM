using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f;

    private Queue<string> sentences;
    private bool isTyping = false;

    [Header("Khóa Script")]
    private vThirdPersonInput inputScript;
    private PlayerAttackController attackScript;
    private Animator playerAnimator;

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            inputScript = player.GetComponent<vThirdPersonInput>();
            attackScript = player.GetComponent<PlayerAttackController>();
            playerAnimator = player.GetComponentInChildren<Animator>();
        }
    }

    public void StartDialogue(List<string> dialogueLines)
    {
        dialoguePanel.SetActive(true);
        TogglePlayerScripts(false);

        sentences.Clear();
        foreach (string line in dialogueLines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping) return;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        TogglePlayerScripts(true);
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            DisplayNextSentence();
        }
    }

    void TogglePlayerScripts(bool state)
    {
        if (inputScript != null) inputScript.enabled = state;
        if (attackScript != null) attackScript.enabled = state;

        if (!state && playerAnimator != null)
        {
            playerAnimator.SetFloat("InputMagnitude", 0f);
            playerAnimator.SetFloat("Vertical", 0f);
            playerAnimator.SetFloat("Horizontal", 0f);
            playerAnimator.SetBool("Attack", false); 
        }
    }
}
