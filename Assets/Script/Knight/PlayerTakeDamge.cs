using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamge : MonoBehaviour
{
    public static PlayerTakeDamge Instance;
    Animator aim;

    public int stunResistance;
    public float stunResistanceHealthCD;
    float timeCD;
    // Start is called before the first frame update
    void Start()
    {
        aim = GetComponent<Animator>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        timeCD += Time.deltaTime;
        if(stunResistance < 100 & timeCD >= stunResistanceHealthCD)
        {
            stunResistance = 100;
            timeCD = 0;
        }
    }

    public void TakeDamge(int damge, int stunNumber)
    {
        GameSession.Instance.TakeDamage(damge);
        stunResistance -= stunNumber;
        timeCD = 0;
        if(stunResistance <= 0)
        {
            aim.SetTrigger("Hit");
            GetComponent<PlayerAttackController>().ClosestEnemy();
        }
    }
    public void Death()
    {
        aim.SetBool("IsDeath", true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponent<vThirdPersonInput>().enabled = false;
        GetComponent<vThirdPersonController>().enabled = false;
        GetComponent<PlayerAttackController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
}
