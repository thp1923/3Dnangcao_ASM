using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField]
    Animator playerAim;
    Rigidbody rb;

    
    public vThirdPersonController tcp;

    public static bool CursorLocked = true;
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;
    public bool isEquipping;

    public bool isAttacking;
    public bool isUntil;
    public bool isBlock;
    private float timeSinceAttack;
    private float timeSinceBlock;
    private float timeSinceUntil;

    public GameObject efAttack;
    public GameObject efUntil;
    public GameObject Light;

    float Drag;
    float AngDrag;

    public static PlayerAttackController Instance;

    public bool canRecceiveInput;
    public bool inputRecceived;

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
        InputManager();
    }

    // Update is called once per frame
    void Update()
    {
        AttackCombo();
        timeSinceBlock += Time.deltaTime;
        timeSinceUntil += Time.deltaTime;
        canRecceiveInput = true;
        ActiveWeapon();
        Until();
        UpdateCursorLock();
        Block();
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
        if (Input.GetMouseButtonDown(0) && playerAim.GetBool("IsGrounded") && CursorLocked && !isUntil && !isBlock)
        {

            if (canRecceiveInput)
            {
                isEquipping = true;
                inputRecceived = true;
                canRecceiveInput = false;
                isAttacking = true;
            }
            else
            {
                return;
            }
        }
    }
    

    void Until()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerAim.GetBool("IsGrounded") && timeSinceUntil > 2f && CursorLocked)
        {
            if(isAttacking) return;
            playerAim.SetTrigger("Until");
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
        timeSinceUntil = 0;
        Invoke(nameof(EndUntil), 4f);
    }
    void EndUntil()
    {
        efUntil.SetActive(false);
    }

    void Block()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerAim.GetBool("IsGrounded") && timeSinceBlock > 3f && CursorLocked)
        {
            isEquipping = true;
            playerAim.SetBool("Block", true);
            Invoke(nameof(UnBlock), 1f);
        }
    }
    void UnBlock()
    {
        playerAim.SetBool("Block", false);
        timeSinceBlock = 0;
    }

    public void LockMove()
    {
        if (isAttacking || isBlock || isUntil || !CursorLocked)
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
    void UpdateCursorLock()
    {
        if (CursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //UnlockMove();
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                CursorLocked = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //LockMove();
            

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                CursorLocked = true;
            }
        }
    }
}
