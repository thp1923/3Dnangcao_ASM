using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyTakeDamge : MonoBehaviour
{
    Animator aim;
    public GameObject me;
    Audio audioE;

    public Slider HpBar;
    public Slider delayHpBar;
    public TMPro.TextMeshProUGUI HpPhanTram;
    float PhanTramHp;

    public int HpMax;
    int Hp;
    int HpDelay;
    public int stunResistanceMax;
    int stunResistance;
    public float stunResistanceHealthCD;
    float timeCD;

    public GameObject DamPopUp;

    public float Y;

    [Header("Time")]
    public float timeDelayHp;
    float _timeDelayHp;

    int HpLost;

    public Transform HitPoint;

    public GameObject HitEffect;

    [Header("---------Items Drop-----------")]
    public List<GameObject> itemsDrop;
    // Start is called before the first frame update
    void Start()
    {
        audioE = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
        Hp = HpMax;
        HpDelay = Hp;
        PhanTramHp = (Hp /(float)HpMax) * 100;
        HpBar.maxValue = HpMax;
        HpBar.value = HpMax;
        delayHpBar.maxValue = HpMax;
        delayHpBar.value = HpMax;
        stunResistance = stunResistanceMax;
        aim = GetComponent<Animator>();
        HpPhanTram.text = PhanTramHp.ToString("F0") + "%";
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
            HpDelay -= (int)(HpLost / (float)4);
            _timeDelayHp = timeDelayHp;
        }
    }

    public void TakeDamge(int damge, int stunNumber)
    {
        Hp -= damge;
        stunResistance -= stunNumber;
        HpLost = damge;
        HpBar.value = Hp;
        timeCD = stunResistanceHealthCD;
        PhanTramHp = (Hp /(float) HpMax) * 100;
        audioE.PlayClip(0);
        HpPhanTram.text = PhanTramHp.ToString("F0") + "%";
        GameObject instance = Instantiate(DamPopUp, transform.position
            + new Vector3(UnityEngine.Random.Range(-1f, 1f), Y, UnityEngine.Random.Range(-1f, 1f)),
            Quaternion.identity);
        instance.GetComponentInChildren<TextMeshProUGUI>().text = damge.ToString();
        Instantiate(HitEffect, HitPoint.position, Quaternion.identity);
        if (Hp <= 0)
        {
            aim.SetBool("IsChasing", false);
            aim.SetBool("IsPatrolling", false);
            aim.SetBool("IsRunning", false);
            aim.SetBool("IsDeath", true);
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            HpPhanTram.text = "0" + "%";
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
        foreach(GameObject items in itemsDrop)
        {
            Instantiate(items, gameObject.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), Quaternion.identity);
        }
        Destroy(me);
    }
}
