using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject DamPopUp;

    [Header("-------------CD----------")]
    public GameObject block;

    public TMPro.TextMeshProUGUI blockCD;
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
        blockCD.text = timeSinceBlock.ToString("F1");
        timeCD -= Time.deltaTime;
        IconBlock();
        if(stunResistance < 100 & timeCD <= 0)
        {
            stunResistance = stunResistanceMax;
            timeCD = stunResistanceHealthCD;
        }
    }

    void IconBlock()
    {
        if(timeSinceBlock > 0) block.SetActive(true);
        else block.SetActive(false);
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
        timeCD = stunResistanceHealthCD;
        GameObject instance = Instantiate(DamPopUp, transform.position
            + new Vector3(UnityEngine.Random.Range(-1f, 1f), 2f, UnityEngine.Random.Range(-1f, 1f)), 
            Quaternion.identity);
        instance.GetComponentInChildren<TextMeshProUGUI>().text = damge.ToString();
        if (stunResistance <= 0)
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
