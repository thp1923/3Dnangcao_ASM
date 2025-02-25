using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : LockMouse
{
    [Header("Main")]
    public static PlayerAttackController Instance;
    public vThirdPersonController tcp;

    [Header("Private")]
    [SerializeField]
    Animator playerAim;
    Rigidbody rb;

    [Header("Attack")]
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;
    public bool isEquipping;

    public bool isAttacking;
    public bool isUntil;
    
    public bool isBuff;
    public float timeCDUntil;
    private float timeSinceAttack;
    private float timeSinceUntil;

    public GameObject efAttack;
    public GameObject efUntil;
    public GameObject Light;

    public bool canRecceiveInput;
    public bool inputRecceived;

    [Header("Move")]
    float Drag;
    float AngDrag;


    [Header("CD")]
    public GameObject nomarl;
    public GameObject skill;

    public TMPro.TextMeshProUGUI skillCD;

    private void Awake()
    {
        Instance = this;
    }

    public void UnEquip()
    {
        isEquipping = false;
    }

    void ActiveWeapon()
    {
        if (isEquipping)
        {
            sword.SetActive(true);
            swordOnShoulder.SetActive(false);
            playerAim.SetBool("Equipping", true);
        }
        else
        {
            sword.SetActive(false);
            swordOnShoulder.SetActive(true);
            playerAim.SetBool("Equipping", false);
        }
    }
    
    public void ResetAttack()
    {
        isAttacking = false;
        isUntil = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Drag = rb.drag;
        AngDrag = rb.angularDrag;
        canRecceiveInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        skillCD.text = timeSinceUntil.ToString("F1");
        IconAttack();
        AttackCombo();
        timeSinceUntil -= Time.deltaTime;
        ActiveWeapon();
        Until();
        UpdateCursorLock();
        LockMove();
    }

    void IconAttack()
    {
        if(timeSinceUntil > 0) skill.SetActive(true);
        else skill.SetActive(false);
        if (!canRecceiveInput) nomarl.SetActive(true);
        else nomarl.SetActive(false);
    }

    public void InputManager()
    {
        if (!canRecceiveInput)
        {
            canRecceiveInput = true;
        }
        else
        {
            canRecceiveInput = false;
        }
    }

    public void Effect(int number)
    {
        if(number == 0)
            efAttack.SetActive(true);
        else 
            efAttack.SetActive(false);
    }
    public void AttackCombo()
    {
        if (Input.GetMouseButtonDown(0) && playerAim.GetBool("IsGrounded") && CursorLocked && !isUntil && !GetComponent<PlayerTakeDamge>().isBlock)
        {

            if (canRecceiveInput)
            {
                isEquipping = true;
                inputRecceived = true;
                canRecceiveInput = false;
                isAttacking = true;
                GetComponent<PlayerAim>().ClosestEnemy();
            }
            else
            {
                return;
            }
        }
    }
    

    void Until()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerAim.GetBool("IsGrounded") && timeSinceUntil <= 0 && CursorLocked)
        {
            if(isAttacking) return;
            GetComponent<AudioPlayer>().PlayAudio(5);
            playerAim.SetTrigger("Until");
            GetComponent<PlayerAim>().ClosestEnemy();
        }
    }
    
    public void UntilAim()
    {
        isEquipping = true;
        isUntil = true;
        efUntil.SetActive(true);
        Vector3 spawnPosition = new Vector3(sword.transform.position.x, sword.transform.position.y + 5, sword.transform.position.z);
        Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
        Instantiate(Light, spawnPosition, spawnRotation);
        timeSinceUntil = timeCDUntil;
        Invoke(nameof(EndUntil), 4f);
    }
    void EndUntil()
    {
        efUntil.SetActive(false);
    }

    
    

    public void LockMove()
    {
        if (isAttacking || GetComponent<PlayerTakeDamge>().isBlock || isUntil || !CursorLocked || isBuff)
        {
            tcp.lockMovement = true;
            tcp.lockRotation = true;
            rb.angularDrag = 100;
            rb.drag = 100;
            playerAim.SetFloat("InputMagnitude", -1f);
        }
        else
            UnlockMove();
    }
    public void UnlockMove()
    {
        
        rb.angularDrag = AngDrag;
        rb.drag = Drag;
        tcp.lockMovement = false;
        tcp.lockRotation = false;
    }
    
}
