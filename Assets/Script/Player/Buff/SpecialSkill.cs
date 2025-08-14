using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    internal int SpecialSkillId;
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
    public GameObject CD_Panal;
    public TextMeshProUGUI cdText;
    float _CD;

    public float damgeRefTime;
    float _damgeRefTime;
    [Header("-----Green Fire-----")]
    public ParticleSystem[] fireEffect;

    public float skillDamgeWolf;

    public float damgeTakeNerf;

    public float rangeGreenFire;

    public Transform wolfSpawnPoint;

    public ParticleSystem fireWolf;

    public GameObject Wolf;

    bool isGreenFire = false;



    [Header("-----Dragon Fire-----")]

    public ParticleSystem[] fireDragonEffect;

    public Transform[] firePoints;

    public float skillFireDragonDamge;

    public float damgeBonus;

    public float rangeDragonFire;

    public GameObject dragonWings;

    public ParticleSystem DragonTrans;

    public Transform dragonSpawnPoint;

    public GameObject Dragon;

    bool isDragonFire = false;

    private void Start()
    {
        CD_Panal.SetActive(false);
        fireWolf.Stop();
        skillTpye = (SpecialSkillTpye)SpecialSkillId;
        atkPlayer = GetComponent<AttackDamgePlayer>();
        ptdPlayer = GetComponent<PlayerTakeDamge>();
        animator = GetComponent<Animator>();
        Wolf.SetActive(false);
        Dragon.SetActive(false);
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
        if (CD_Panal.activeSelf)
            cdText.text = _CD.ToString("F1");
        if (_CD > 0)
        {
            CD_Panal.SetActive(true);
        }
        else
        {
            CD_Panal.SetActive(false);
        }
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
                WolfSpawn();
                break;
            case SpecialSkillTpye.DragonFire:
                isDragonFire = true;
                DragonSpawn();
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

    public void WolfSpawn()
    {
        Wolf.transform.position = new Vector3(wolfSpawnPoint.position.x, transform.position.y, wolfSpawnPoint.position.z);
        Wolf.SetActive(true);
    }

    public void DragonSpawn()
    {
        Dragon.transform.position = new Vector3(dragonSpawnPoint.position.x, transform.position.y, dragonSpawnPoint.position.z);
        Dragon.SetActive(true);
    }

    IEnumerator EndSkill()
    {
        yield return new WaitForSeconds(CD/2f);
        if (isGreenFire)
        {
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
            WolfSpawn();
        }
        else if (isDragonFire)
        {
            isDragonFire = false;
            DragonSpawn();
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
        }
    }

    public void GreenFireDamge()
    {
        _damgeRefTime -= Time.deltaTime;
        int damge = (int)(atkPlayer.BaseATK * (skillDamgeWolf/100f));
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
        yield return new WaitForSeconds(CD / 2f - 1f);
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
