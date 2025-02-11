using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCSystem : MonoBehaviour
{
    bool player_detection = false;
    public NPCConversation con;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player_detection && Input.GetKeyDown(KeyCode.F))
        {
            ConversationManager.Instance.StartConversation(con);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player_detection = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        player_detection = false;
    }
}
