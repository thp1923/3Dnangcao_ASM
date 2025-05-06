using Invector.vCharacterController;
using StatsManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTakeDamge : StatsAlive
{
    Animator PlayerAim;
    Rigidbody rb;

    public bool isBlock;
    public bool isDeath;
    public bool noTakeDamge;

    public GameObject DamPopUp;

    [Header("-------------CD----------")]

    public Audio audioP;

    [Header("Test")]
    public int stunDamgeTest;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PlayerAim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Block();
        if(Input.GetKeyDown(KeyCode.J)) // Test take damge
            TakeDamge( 10000 ,stunDamgeTest);
    }
    
    
    void Block()
    {
        if (Input.GetKey(KeyCode.Mouse1) && PlayerAim.GetBool("IsGrounded") && PlayerAttackController.CursorLocked)
        {
            //audioP.PlayClip(9);
            PlayerAim.SetBool("IsBlock", true);
            isBlock = true;
        }
        else
        {
            isBlock = false;
            PlayerAim.SetBool("IsBlock", false);
        }
    }

    public override void TakeDamge(int damge, int stunDamge)
    {
        if (noTakeDamge) return;
        if (isBlock)
        {
            PlayerAim.SetTrigger("Hit");
            return;
        }
        base.TakeDamge(damge, stunDamge);
        audioP.PlayClip(7);
        if(currentHP <= 0)
        {
            Death();
        }
        if(stunDamge > StunResistance)
        {
            int stun = stunDamge - StunResistance;
            if(stun > 100)
            {
                PlayerAim.SetTrigger("Hit3");
            }
            else if(stun < 100 && stun >=50) 
                PlayerAim.SetTrigger("Hit2");
            else
                PlayerAim.SetTrigger("Hit");
        }
    }
    public void Death()
    {
        audioP.PlayClip(10);
        PlayerAim.SetBool("IsDeath", true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isDeath = true;
        GetComponent<vThirdPersonInput>().enabled = false;
        GetComponent<vThirdPersonController>().enabled = false;
        GetComponent<PlayerAttackController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        rb.useGravity = false;
    }
}
