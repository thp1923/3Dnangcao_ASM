using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamge : MonoBehaviour
{
    public static PlayerTakeDamge Instance;
    Animator aim;
    public int stunResistanceMax;
    int stunResistance;
    public float stunResistanceHealthCD;
    float timeCD;

    public bool noTakeDamge;
    // Start is called before the first frame update
    void Start()
    {
        aim = GetComponent<Animator>();
        Instance = this;
        stunResistance = stunResistanceMax;
    }

    // Update is called once per frame
    void Update()
    {
        timeCD += Time.deltaTime;
        if(stunResistance < 100 & timeCD >= stunResistanceHealthCD)
        {
            stunResistance = stunResistanceMax;
            timeCD = 0;
        }
    }

    public void TakeDamge(int damge, int stunNumber)
    {
        if (noTakeDamge) return;
        GameSession.Instance.TakeDamage(damge);
        stunResistance -= stunNumber;
        timeCD = 0;
        if(stunResistance <= 0)
        {
            aim.SetTrigger("Hit");
            GetComponent<PlayerAim>().ClosestEnemy();
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
