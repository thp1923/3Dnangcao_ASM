using Invector.vCharacterController;
using StatsManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTakeDamge : StatsAlive
{
    Animator anim;               // Animator của Player
    PlayerAim playerAim;         // Script PlayerAim
    Rigidbody rb;

    public AudioSource parrySource;
    public AudioClip parryClip;

    internal bool GodMode;
    internal bool isBlock;
    internal bool isDeath;
    internal bool noTakeDamge;
    public ParticleSystem[] fireTrails;
    [SerializeField] private KeyCode blockKey = KeyCode.Mouse1;
    public int staminaLost = 35;
    public Animator CanvaDied;

    public Material defMaterial;
    public GameObject targetRoot;
    public List<SkinnedMeshRenderer> skinnedMeshes;

    [Header("---------Knock Back----------")]
    public float[] knockbackForce;
    public float[] knockBackTime;
    private Coroutine knockbackRoutine;

    [Header("-------------Heath----------")]
    [SerializeField] private KeyCode heathKey = KeyCode.R;
    int heath;
    public int heathCount;
    internal int _heathCount;
    public ParticleSystem healFire;
    public Transform healTransformEffect;
    bool isHealling;
    public float timeHeal;
    public AudioSource heallSound;
    public AudioClip[] healClip;
    public TextMeshProUGUI healText;

    [Header("-------------Shake----------")]
    public float[] duration; // Time shake
    public float[] magnitude; // Shake level

    public static int MaxHp { get; internal set; }

    protected override void Start()
    {
        base.Start();
        healFire.Stop();
        healText.gameObject.SetActive(false);

        foreach (var fireTrail in fireTrails)
        {
            fireTrail.gameObject.SetActive(false);
        }

        anim = GetComponent<Animator>();
        playerAim = GetComponent<PlayerAim>();
        rb = GetComponent<Rigidbody>();
        _heathCount = heathCount;
        Collect();
    }

    protected override void Update()
    {
        base.Update();
        Block();
        Heath();
    }

    void Heath()
    {
        if (_heathCount <= 0 || isDeath) return;

        if (Input.GetKeyDown(heathKey) && !isHealling)
        {
            HeathHp();
        }
    }

    public void ParryAudio()
    {
        parrySource.PlayOneShot(parryClip);
    }

    public void HeathHp()
    {
        heath = (int)(MaxHP * 0.3f);
        healFire.transform.position = healTransformEffect.position;
        currentHP += heath;
        isHealling = true;
        _heathCount -= 1;
        healText.gameObject.SetActive(true);
        healText.text = _heathCount.ToString();
        heallSound.PlayOneShot(healClip[0]);
        healFire.Play();

        if (currentHP >= MaxHP)
            currentHP = MaxHP;

        HpSlider.value = currentHP;
        StartCoroutine(Healling());
    }

    public void PlayFlame(bool IsFlame)
    {
        if (fireTrails == null) return;
        foreach (var fireTrail in fireTrails)
        {
            fireTrail.gameObject.SetActive(IsFlame);
        }
    }

    IEnumerator Healling()
    {
        yield return new WaitForSeconds(timeHeal);
        healFire.Stop();
        healText.gameObject.SetActive(false);
        heallSound.PlayOneShot(healClip[1]);
        isHealling = false;
    }

    void Block()
    {
        if (Input.GetKeyDown(blockKey)
            && anim.GetBool("IsGrounded")
            && PlayerAttackController.CursorLocked
            && !isBlock
            && GetComponent<Stamina>().stamina >= staminaLost)
        {
            anim.SetTrigger("Block");
            GetComponent<Stamina>().TakeStamina(staminaLost);
        }
    }

    void Collect()
    {
        if (targetRoot == null) return;

        skinnedMeshes.Clear();
        skinnedMeshes.AddRange(targetRoot.GetComponentsInChildren<SkinnedMeshRenderer>());
    }

    public void BlockEffect(bool block)
    {
        if (block)
        {
            foreach (var renderer in skinnedMeshes)
            {
                if (System.Array.Exists(renderer.materials, mat => mat == defMaterial))
                    continue;

                Material[] currentMaterials = renderer.materials;
                Material[] newMaterials = new Material[currentMaterials.Length + 1];

                for (int i = 0; i < currentMaterials.Length; i++)
                    newMaterials[i] = currentMaterials[i];

                newMaterials[currentMaterials.Length] = defMaterial;
                renderer.materials = newMaterials;
            }
        }
        else
        {
            foreach (var renderer in skinnedMeshes)
            {
                Material[] mats = renderer.materials;
                if (mats.Length == 0) continue;

                Material[] newMats = new Material[mats.Length - 1];
                for (int i = 0; i < newMats.Length; i++)
                    newMats[i] = mats[i];

                renderer.materials = newMats;
            }
        }
    }

    public override void TakeDamge(int damge, int stunDamge, int trueDamge)
    {
        if (GodMode || GetComponent<PlayerDodge>().isDodging) return;

        if (isBlock)
        {
            base.TakeDamge(0, 0, trueDamge);
            return;
        }

        base.TakeDamge(damge, stunDamge, trueDamge);

        if (currentHP <= 0)
        {
            Death();
        }

        if (stunDamge > (StunResistance + stunResistanceBonus))
        {
            if (anim == null) return;

            int stun = stunDamge - (StunResistance + stunResistanceBonus);

            playerAim?.ClosestEnemy();
            playerAim?.LockForStun();
            if (stun >= 100)
            {
                anim.SetTrigger("Hit3");
                CameraShake.Instance.StartShake(duration[2], magnitude[2]);
                ApplyKnockback(knockbackForce[2], knockBackTime[2]);
            }
            else if (stun < 100 && stun >= 50)
            {
                anim.SetTrigger("Hit2");
                CameraShake.Instance.StartShake(duration[1], magnitude[1]);
                ApplyKnockback(knockbackForce[1], knockBackTime[1]);
            }
            else
            {
                anim.SetTrigger("Hit");
                CameraShake.Instance.StartShake(duration[0], magnitude[0]);
                ApplyKnockback(knockbackForce[0], knockBackTime[0]);
            }
        }
    }

    public void ApplyKnockback(float knockForce, float lockDuration)
    {
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
        if (anim == null) return;
        CanvaDied.gameObject.SetActive(true);
        isDeath = true;
        anim.SetBool("IsDeath", true);
        CanvaDied.SetBool("IsDeath", true);
        anim.SetFloat("InputMagnitude", -1f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GetComponent<vThirdPersonInput>().enabled = false;
        GetComponent<vThirdPersonController>().enabled = false;
        GetComponent<PlayerAttackController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        rb.useGravity = false;
    }
}
