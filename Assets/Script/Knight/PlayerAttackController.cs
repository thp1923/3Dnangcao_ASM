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

    public bool canClick;

    private void Awake()
    {
        Instance = this;
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
        canRecceiveInput = true;
        canClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        AttackCombo();
        timeSinceUntil -= Time.deltaTime;
        Until();
        UpdateCursorLock();
        LockMove();
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
        if (Input.GetMouseButtonDown(0) && playerAim.GetBool("IsGrounded") && CursorLocked && !GetComponent<PlayerTakeDamge>().isBlock && canClick)
        {

            if (canRecceiveInput)
            {
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
        if (isAttacking || isUntil || !CursorLocked || GetComponent<PlayerTakeDamge>().isStun)
        {
            tcp.lockMovement = true;
            tcp.lockRotation = true;
            playerAim.SetFloat("InputMagnitude", -1f);
        }
        else
            UnlockMove();
    }
    public void UnlockMove()
    {
        tcp.lockMovement = false;
        tcp.lockRotation = false;
    }
    
}
