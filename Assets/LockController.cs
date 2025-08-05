using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [Header("---In Player----")]
    public GameObject Inventory;
    public GameObject Quest;
    [HideInInspector] public bool isInven;
    [HideInInspector] public bool isQuest;
    [HideInInspector] public bool isOut;
    [Header("----Buttom Lock----")]
    public KeyCode keyInventory = KeyCode.B;
    public KeyCode keyQuest = KeyCode.J;
    PlayerAttackController attackCtrl;
    // Start is called before the first frame update
    void Start()
    {
        attackCtrl = GetComponent<PlayerAttackController>();
        if(Inventory != null) Inventory.SetActive(false);
        if(Quest != null) Quest.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Check();
        InPlayerController();
    }

    public void InPlayerController()
    {
        if (Input.GetKeyDown(keyInventory) && !isOut)
        {
            if (Inventory.activeSelf)
            {
                Inventory.SetActive(false);
                isInven = false;
                attackCtrl.LockController(true);
            }
            else
            {
                Inventory.SetActive(true);
                isInven = true; 
                attackCtrl.LockController(false);
            }
        }
        if (Input.GetKeyDown(keyQuest) && !isOut)
        {
            if (Quest.activeSelf)
            {
                Quest.SetActive(false);
                isQuest = false;
                attackCtrl.LockController(true);
            }
            else
            {
                Quest.SetActive(true);
                isQuest = true;
                attackCtrl.LockController(false);
            }
        }
    }

    public void OutPlayerController()
    {
        if(!isQuest || !isInven)
        {
            attackCtrl.LockController(false);
            isOut = true;
        }
        else
        {
            isOut = false;
            attackCtrl.LockController(true);
        }
    }

    void Check()
    {
        if (isQuest)
        {
            Inventory.SetActive(false);
            isInven = false;
        }
        if (isInven)
        {
            Quest.SetActive(false);
            isQuest = false;
        }
    }
}
