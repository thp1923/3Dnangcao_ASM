using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour
{
    AttackDamgePlayer atkPlayer;
    PlayerTakeDamge ptdPlayer;
    internal bool canSkill;
    public enum SpecialSkillTpye
    {
        GreenFire = 0, DragonFire = 1
    }

    protected int SpecialSkillId;
    [Header("-----Skill Controller-----")]

    public SpecialSkillTpye skillTpye;

    public KeyCode SpecialSkillKey = KeyCode.Q;

    Animator animator;
    public LayerMask attackMask;

    public AudioSource buffSource;
    public AudioSource fireLoop;
    public AudioClip[] skillClip;
    public AudioClip[] Loop;

    [Header("--------CD--------")]
    public float CD;
    float _CD;

    public float damgeRefTime;
    float _damgeRefTime;
    [Header("-----Green Fire-----")]
    public ParticleSystem[] fireEffect;

    internal float skillDamge;

    internal float damgeTakeNerf;

    public float rangeGreenFire;


    bool isGreenFire = false;



    [Header("-----Dragon Fire-----")]

    public ParticleSystem[] fireDragonEffect;

    public Transform[] firePoints;

    internal float skillFireDragonDamge;

    internal float damgeBonus;

    public float rangeDragonFire;

    public GameObject dragonWings;

    public ParticleSystem DragonTrans;

    bool isDragonFire = false;

    private void Start()
    {
        skillTpye = (SpecialSkillTpye)SpecialSkillId;
        atkPlayer = GetComponent<AttackDamgePlayer>();
        ptdPlayer = GetComponent<PlayerTakeDamge>();
        animator = GetComponent<Animator>();
        foreach(var ef in fireEffect)
        {
            ef.Stop();
            ef.GetComponent<Light>().enabled = false;
        }
        foreach(var ef in fireDragonEffect)
        {
            if (ef != null)
            {
                ef.Stop();
                var light = ef.GetComponent<Light>();
                if (light != null)
                    light.enabled = false;
            }
        }
        dragonWings.SetActive(false);
        if (DragonTrans != null)
        {
            DragonTrans.Stop();
            var light = DragonTrans.GetComponent<Light>();
            if (light != null)
                light.enabled = false;
        }
    }
    private void Update()
    {
        SpecialSkillController();
        GreenFireDamge();
        DragonFireDamge();
    }
    public void SpecialSkillController()
    {
        _CD -= Time.deltaTime;
        if (Input.GetKeyDown(SpecialSkillKey) && _CD <= 0 && canSkill)
        {
            _CD = CD;
            animator.SetTrigger("Skill");
        }
    }
    public void PlayAudio()
    {
        switch (skillTpye)
        {
            case SpecialSkillTpye.GreenFire:
                buffSource.PlayOneShot(skillClip[0]);
                break;
            case SpecialSkillTpye.DragonFire:
                buffSource.PlayOneShot(skillClip[1]);
                break;
            default:
                break;
        }
    }
    public void BeginSkill()
    {
        SpecialSkillId = (int)skillTpye;
        switch (skillTpye)
        {
            case SpecialSkillTpye.GreenFire:
                isGreenFire = true;
                ptdPlayer.damgeTake += damgeTakeNerf;
                fireLoop.clip = Loop[0];
                fireLoop.pitch = 1f;
                fireLoop.Play();
                foreach (var ef in fireEffect)
                {
                    if (ef != null)
                    {
                        ef.Play();
                        var light = ef.GetComponent<Light>();
                        if (light != null)
                        {
                            light.enabled = true;
                        }
                    }
                }
                break;
            case SpecialSkillTpye.DragonFire:
                isDragonFire = true;
                atkPlayer.damgeAttack += damgeBonus;
                fireLoop.clip = Loop[0];
                fireLoop.pitch = 1f;
                fireLoop.Play();
                buffSource.PlayOneShot(skillClip[1]);
                for (int i = 0; i < fireDragonEffect.Length; i++)
                {
                    if (i < firePoints.Length && fireDragonEffect[i] != null && firePoints[i] != null)
                    {
                        fireDragonEffect[i].transform.position = firePoints[i].position;
                        fireDragonEffect[i].Play();

                        var light = fireDragonEffect[i].GetComponent<Light>();
                        if (light != null)
                        {
                            light.enabled = true;
                        }
                    }
                }
                StartCoroutine(DragonFire());
                dragonWings.SetActive(true);
                break;
            default:
                break;
        }

        StartCoroutine(EndSkill());
        _CD = CD;
    }

    IEnumerator EndSkill()
    {
        yield return new WaitForSeconds(CD/3);
        switch (skillTpye)
        {
            case SpecialSkillTpye.GreenFire:
                isGreenFire = false;
                fireLoop.Stop();
                ptdPlayer.damgeTake -= damgeTakeNerf;
                foreach (var ef in fireEffect)
                {
                    if (ef != null)
                    {
                        ef.Stop();
                        var light = ef.GetComponent<Light>();
                        if (light != null)
                            light.enabled = false;
                    }
                }
                break;
            case SpecialSkillTpye.DragonFire:
                isDragonFire = false;
                atkPlayer.damgeAttack -= damgeBonus;
                foreach (var ef in fireDragonEffect)
                {
                    if (ef != null)
                    {
                        ef.Stop();
                        var light = ef.GetComponent<Light>();
                        if (light != null)
                            light.enabled = false;
                    }
                }
                dragonWings.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void GreenFireDamge()
    {
        _damgeRefTime -= Time.deltaTime;
        int damge = (int)(atkPlayer.BaseATK * (skillDamge/100f));
        if(isGreenFire && _damgeRefTime <= 0)
        {
            _damgeRefTime = damgeRefTime;
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, rangeGreenFire, attackMask);
            foreach(Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyTakeDamge>().TakeDamge(0, 0, damge);
            }
        }
    }

    IEnumerator DragonFire()
    {
        yield return new WaitForSeconds(CD / 3 - 1f);
        fireLoop.clip = Loop[1];
        fireLoop.pitch = 2f;
        fireLoop.Play();
        DragonTrans.Play();
        DragonTrans.GetComponent<Light>().enabled = true;
        StartCoroutine(DragonFire2());
    }

    IEnumerator DragonFire2()
    {
        yield return new WaitForSeconds(2f);
        if (DragonTrans != null)
        {
            fireLoop.Stop();
            DragonTrans.Stop();
            var light = DragonTrans.GetComponent<Light>();
            if (light != null)
                light.enabled = false;
        }
    }

    public void DragonFireDamge()
    {
        _damgeRefTime -= Time.deltaTime;
        int damge = (int)(atkPlayer.BaseATK * (skillFireDragonDamge / 100f));
        if (isDragonFire && _damgeRefTime <= 0)
        {
            _damgeRefTime = damgeRefTime;
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, rangeDragonFire, attackMask);
            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyTakeDamge>().TakeDamge(0, 0, damge);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangeGreenFire);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeDragonFire);
    }
}
