using Invector.vCharacterController;
using System.Collections;
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

    internal bool isAttacking;


    public bool canRecceiveInput;
    public bool inputRecceived;

    public bool canClick;

    bool swordContract;

    [SerializeField] Vector2 contract_speed_time = new Vector2(0.1f, 0.1f);

    private void Awake()
    {
        Instance = this;
    }
    
    public void SwordContract()
    {
        if (!swordContract) StartCoroutine("SpeedRegain"); else return;
    }

    private IEnumerator SpeedRegain()
    {
        swordContract = true;
        playerAim.speed = contract_speed_time.x;
        yield return new WaitForSeconds(contract_speed_time.y);
        playerAim.speed = 1;
        swordContract = false;
    }

    public void ResetAttack()
    {
        isAttacking = false;
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
        UpdateCursorLock();
        //LockMove();
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

    
    

    //public void LockMove()
    //{
    //    if (isAttacking || isUntil || !CursorLocked || GetComponent<PlayerTakeDamge>().isStun)
    //    {
    //        tcp.lockMovement = true;
    //        tcp.lockRotation = true;
    //        playerAim.SetFloat("InputMagnitude", -1f);
    //    }
    //    else
    //        UnlockMove();
    //}
    //public void UnlockMove()
    //{
    //    tcp.lockMovement = false;
    //    tcp.lockRotation = false;
    //}
    
}
