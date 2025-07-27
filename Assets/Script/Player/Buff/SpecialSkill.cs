using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour
{
    AttackDamgePlayer atkPlayer;
    PlayerTakeDamge ptdPlayer;
    internal bool canBuff;
    public enum SpecialSkillTpye
    {
        GreenFire = 0, DragonFire = 1
    }
    [Header("-----Skill Controller-----")]

    public SpecialSkillTpye skillTpye;

    public KeyCode SpecialSkillKey = KeyCode.Q;

    Animator animator;
    public LayerMask attackMask;

    [Header("--------CD--------")]
    public float CD;
    float _CD;

    [Header("-----Green Fire-----")]
    public ParticleSystem[] fireEffect;

    protected int skillDamge;

    protected float damgeTakeNerf;

    public float rangeGreenFire;

    public float damgeRefTime;
    float _damgeRefTime;

    bool isGreenFire = false;



    //[Header("-----Dragon Fire-----")]

    //public ParticleSystem[] fireDragonEffect;

    private void Start()
    {
        atkPlayer = GetComponent<AttackDamgePlayer>();
        ptdPlayer = GetComponent<PlayerTakeDamge>();
        animator = GetComponent<Animator>();
        foreach(var ef in fireEffect)
        {
            ef.Stop();
            ef.GetComponent<Light>().enabled = false;
        }
    }
    private void Update()
    {
        SpecialSkillController();
        GreenFireDamge();
    }
    public void SpecialSkillController()
    {
        if (Input.GetKeyDown(SpecialSkillKey) && _CD <= 0 /*&& canBuff*/)
        {
            _CD = CD;
            switch (skillTpye)
            {
                case SpecialSkillTpye.GreenFire:
                    animator.SetTrigger("BuffGreenFire");
                    break;
                case SpecialSkillTpye.DragonFire:
                    animator.SetTrigger("BuffDragonFire");
                    break;
                default:
                    break;
            }
        }
    }
    public void BeginSkill()
    {
        switch (skillTpye)
        {
            case SpecialSkillTpye.GreenFire:
                isGreenFire = true;
                ptdPlayer.damgeTake += damgeTakeNerf;
                foreach (var ef in fireEffect)
                {
                    ef.Play();
                    ef.GetComponent<Light>().enabled = true;
                }
                break;
            case SpecialSkillTpye.DragonFire:
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
                ptdPlayer.damgeTake -= damgeTakeNerf;
                foreach (var ef in fireEffect)
                {
                    ef.Stop();
                    ef.GetComponent<Light>().enabled = false;
                }
                break;
            case SpecialSkillTpye.DragonFire:
                break;
            default:
                break;
        }
    }

    public void GreenFireDamge()
    {
        _damgeRefTime -= Time.deltaTime;
        int damge = (int)(atkPlayer.BaseATK * ((100+skillDamge)/100f));
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangeGreenFire);
    }
}
