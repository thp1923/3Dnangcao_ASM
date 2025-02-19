using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyTakeDamge : MonoBehaviour
{
    Animator aim;
    public GameObject me;

    public Slider HpBar;
    public Slider delayHpBar;

    public int HpMax;
    int Hp;
    int HpDelay;
    public int stunResistanceMax;
    int stunResistance;
    public float stunResistanceHealthCD;
    float timeCD;

    [Header("Time")]
    public float timeDelayHp;
    float _timeDelayHp;

    public int HpLost;
    // Start is called before the first frame update
    void Start()
    {
        Hp = HpMax;
        HpDelay = Hp;
        HpBar.maxValue = HpMax;
        HpBar.value = HpMax;
        delayHpBar.maxValue = HpMax;
        delayHpBar.value = HpMax;
        stunResistance = stunResistanceMax;
        aim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timeCD -= Time.deltaTime;
        DelayHp();
        if (stunResistance < stunResistanceMax & timeCD <= 0)
        {
            RestoreStunRetance();
        }
    }

    void DelayHp()
    {
        _timeDelayHp -= Time.deltaTime;
        if (Hp < HpDelay && _timeDelayHp <= 0)
        {
            delayHpBar.value -= HpLost;
            HpDelay -= HpLost;
            _timeDelayHp = timeDelayHp;
        }
    }

    public void TakeDamge(int damge, int stunNumber)
    {
        Hp -= damge;
        stunResistance -= stunNumber;
        HpBar.value = Hp;
        timeCD = stunResistanceHealthCD;
        if (Hp <= 0)
        {
            aim.SetBool("IsChasing", false);
            aim.SetBool("IsPatrolling", false);
            aim.SetBool("IsRunning", false);
            aim.SetBool("IsDeath", true);
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
        if (stunResistance <= 0)
        {
            aim.SetTrigger("Hit");
        }
    }

    public void RestoreStunRetance()
    {
        timeCD = stunResistanceHealthCD;
        stunResistance = stunResistanceMax;
    }

    void Death()
    {
        FindObjectOfType<PlayerAim>().RemoveEnemy();
        Destroy(me);
    }
}
