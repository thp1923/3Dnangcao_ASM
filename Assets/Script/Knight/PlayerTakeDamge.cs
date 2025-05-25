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

    internal bool isBlock;
    internal bool isDeath;
    internal bool noTakeDamge;

    public GameObject DamPopUp;

    [Header("---------Knock Back----------")]
    public float[] knockbackForce;
    public float[] knockBackTime;
    private Coroutine knockbackRoutine;

    [Header("-------------CD----------")]


    [Header("Test")]
    public int stunDamgeTest;

    [Header("-------------Shake----------")]
    public float[] duration; // Time shake
    public float[] magnitude; // Shake level
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
            TakeDamge( 10000 ,stunDamgeTest, 0);
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
        if(stunDamge > StunResistance)
        {
            if(PlayerAim == null) return;
            int stun = stunDamge - StunResistance;
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
