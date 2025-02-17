using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamge : MonoBehaviour
{
    Animator PlayerAim;
    Rigidbody rb;
    public int stunResistanceMax;
    int stunResistance;
    public float stunResistanceHealthCD;
    float timeCD;
    public float timeSinceBlockCD;
    private float timeSinceBlock;

    public bool isBlock;
    public bool isDeath;
    public bool noTakeDamge;
    // Start is called before the first frame update
    void Start()
    {
        PlayerAim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        stunResistance = stunResistanceMax;
    }

    // Update is called once per frame
    void Update()
    {
        Block();
        timeCD += Time.deltaTime;
        if(stunResistance < 100 & timeCD >= stunResistanceHealthCD)
        {
            stunResistance = stunResistanceMax;
            timeCD = 0;
        }
    }
    void Block()
    {
        timeSinceBlock -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1) && PlayerAim.GetBool("IsGrounded") && timeSinceBlock <= 0 && PlayerAttackController.CursorLocked)
        {
            GetComponent<PlayerAttackController>().isEquipping = true;
            PlayerAim.SetTrigger("Block");
            GetComponent<PlayerAim>().ClosestEnemy();
            timeSinceBlock = timeSinceBlockCD;
        }
    }
    public void TakeDamge(int damge, int stunNumber, float knockBack)
    {
        if (noTakeDamge) return;
        GameSession.Instance.TakeDamage(damge);
        stunResistance -= stunNumber;
        timeCD = 0;
        if(stunResistance <= 0)
        {
            PlayerAim.SetTrigger("Hit");
            PlayerAim.SetFloat("InputMagnitude", -1);
            GetComponent<PlayerAim>().ClosestEnemy();
            GetComponent<PlayerAttackController>().isEquipping = true;
            rb.AddForce(-transform.forward * knockBack);
        }
    }
    public void Death()
    {
        PlayerAim.SetBool("IsDeath", true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isDeath = true;
        GetComponent<vThirdPersonInput>().enabled = false;
        GetComponent<vThirdPersonController>().enabled = false;
        GetComponent<PlayerAttackController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        rb.useGravity = false;
        rb.mass = 10;
    }
}
