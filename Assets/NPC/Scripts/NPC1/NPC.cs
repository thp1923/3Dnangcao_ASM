using DialogueEditor;
using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class NPC : MonoBehaviour
{
    bool player_detected = false;
    public NPCConversation con;
    public GameObject FPanel;

    private vThirdPersonInput inputScript;
    private PlayerAttackController attackScript;
    private PlayerDodge dodgeScript;
    private PlayerTakeDamge playerTakeDamage;
    private Animator playerAnimator;
    private Rigidbody rb;

    private void Start()
    {
        if (FPanel != null)
        {
            FPanel.SetActive(false);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            inputScript = player.GetComponent<vThirdPersonInput>();
            attackScript = player.GetComponent<PlayerAttackController>();
            dodgeScript = player.GetComponent<PlayerDodge>();
            playerTakeDamage = player.GetComponent<PlayerTakeDamge>();
            playerAnimator = player.GetComponentInChildren<Animator>();
            rb = player.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (player_detected && Input.GetKeyDown(KeyCode.F))
        {
            ConversationManager.Instance.StartConversation(con);
            if (FPanel != null)
            {
                FPanel.SetActive(false);
            }
            TogglePlayerScripts(false);
        }
        else if (player_detected && Input.GetKeyDown(KeyCode.Escape))
        {
            ConversationManager.Instance.EndConversation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_detected = true;
            if (FPanel != null)
            {
                FPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_detected = false;
            if (FPanel != null)
            {
                FPanel.SetActive(false);
            }
        }
    }

    public void TogglePlayerScripts(bool state)
    {
        if (inputScript != null) inputScript.enabled = state;
        if (attackScript != null) attackScript.enabled = state;
        if (dodgeScript != null) dodgeScript.enabled = state;
        if (playerTakeDamage != null) playerTakeDamage.enabled = state;

        if (rb != null)
        {
            if (!state)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                                 RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }

        if (!state && playerAnimator != null)
        {
            playerAnimator.SetFloat("InputMagnitude", 0f);
            playerAnimator.SetFloat("Vertical", 0f);
            playerAnimator.SetFloat("Horizontal", 0f);
            playerAnimator.SetBool("Attack", false);
        }
    }
}
