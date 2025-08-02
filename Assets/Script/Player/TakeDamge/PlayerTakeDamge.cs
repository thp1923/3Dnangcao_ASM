using Invector.vCharacterController;
using StatsManager;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerTakeDamge : StatsAlive
{
    Animator PlayerAim;
    Rigidbody rb;

    internal bool isBlock;
    internal bool isDeath;
    internal bool noTakeDamge;
    [SerializeField] private KeyCode blockKey = KeyCode.Mouse1;
    public int staminaLost = 35;
    public Animator CanvaDied;

    [Header("---------Knock Back----------")]
    public float[] knockbackForce;
    public float[] knockBackTime;
    private Coroutine knockbackRoutine;

    [Header("-------------Heath----------")]
    [SerializeField] private KeyCode heathKey = KeyCode.R;
    int heath;
    public int heathCount;
    protected int _heathCount;

    [Header("-------------Shake----------")]
    public float[] duration; // Time shake
    public float[] magnitude; // Shake level

    public static int MaxHp { get; internal set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PlayerAim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _heathCount = heathCount;
    }

    // Update is called once per frame
    void Update()
    {
        Block();
        Heath();
    }
    
    void Heath()
    {
        if (_heathCount <= 0 || isDeath)
        {
            return;
        }
        if (Input.GetKeyDown(heathKey))
        {
            HeathHp();
        }
    }
    
    public void HeathHp()
    {
        heath = (int)(MaxHP * 0.3f);
        currentHP += heath;
        if (currentHP >= MaxHP)
        {
            currentHP = MaxHP;
        }
        heathCount -= 1;
        HpSlider.value = currentHP;
    }

    void Block()
    {
        if (Input.GetKeyDown(blockKey) && PlayerAim.GetBool("IsGrounded") 
            && PlayerAttackController.CursorLocked && !isBlock 
            && GetComponent<Stamina>().stamina >= staminaLost)
        {
            //audioP.PlayClip(9);
            PlayerAim.SetTrigger("Block");
            GetComponent<Stamina>().TakeStamina(staminaLost);
        }
    }

    public override void TakeDamge(int damge, int stunDamge, int trueDamge)
    {
        if (noTakeDamge || GetComponent<PlayerDodge>().isDodging) return;
        if (isBlock)
        {
            PlayerAim.SetTrigger("Hit");
            base.TakeDamge(0, stunDamge, trueDamge);
            return;
        }
        base.TakeDamge(damge, stunDamge, trueDamge);
        if(currentHP <= 0)
        {
            Death();
        }
        if(stunDamge > (StunResistance + stunResistanceBonus))
        {
            if(PlayerAim == null) return;
            int stun = stunDamge - (StunResistance + stunResistanceBonus);
            GetComponent<PlayerAim>().ClosestEnemy();
            if(stun > 4000)
            {
                PlayerAim.SetTrigger("Hit3");
                CameraShake.Instance.StartShake(duration[2], magnitude[2]);
                ApplyKnockback(knockbackForce[3], knockBackTime[3]);
            }
            else if(stun >= 100 && stun <= 4000)
            {
                PlayerAim.SetTrigger("Hit3");
                CameraShake.Instance.StartShake(duration[2], magnitude[2]);
                ApplyKnockback(knockbackForce[2], knockBackTime[2]);
            }
            else if(stun < 100 && stun >=50)
            {
                PlayerAim.SetTrigger("Hit2");
                CameraShake.Instance.StartShake(duration[1], magnitude[1]);
                ApplyKnockback(knockbackForce[1], knockBackTime[1]);
            }
            else
            {
                PlayerAim.SetTrigger("Hit");
                CameraShake.Instance.StartShake(duration[0], magnitude[0]);
                ApplyKnockback(knockbackForce[0], knockBackTime[0]);
            }
        }
    }

    public void ApplyKnockback(float knockForce, float lockDuration)
    {
        // Nếu đang knockback thì dừng cũ trước
        if (knockbackRoutine != null)
            StopCoroutine(knockbackRoutine);

        knockbackRoutine = StartCoroutine(KnockbackCoroutine(knockForce, lockDuration));
    }

    private IEnumerator KnockbackCoroutine(float knockForce, float lockDuration)
    {

        Vector3 knockbackDir = -transform.forward.normalized;
        float timer = 0f;

        while (timer < lockDuration)
        {
            rb.MovePosition(rb.position + knockbackDir * knockForce * Time.fixedDeltaTime);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

    }

    public void CancelKnockback()
    {
        if (knockbackRoutine != null)
        {
            StopCoroutine(knockbackRoutine);
            knockbackRoutine = null;
        }
    }



    public void Death()
    {
        if (PlayerAim == null) return;
        isDeath = true;
        PlayerAim.SetBool("IsDeath", true);
        CanvaDied.SetBool("IsDeath", true);
        PlayerAim.SetFloat("InputMagnitude", -1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponent<vThirdPersonInput>().enabled = false;
        GetComponent<vThirdPersonController>().enabled = false;
        GetComponent<PlayerAttackController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        rb.useGravity = false;
    }
}
