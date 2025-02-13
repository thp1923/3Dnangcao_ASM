using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyTakeDamge : MonoBehaviour
{
    Animator aim;
    public GameObject me;

    public int HpMax;
    int Hp;

    public int stunResistance;
    public float stunResistanceHealthCD;
    float timeCD;
    // Start is called before the first frame update
    void Start()
    {
        Hp = HpMax;
        aim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timeCD += Time.deltaTime;
        if (stunResistance < 100 & timeCD >= stunResistanceHealthCD)
        {
            stunResistance = 100;
            timeCD = 0;
        }
    }
    public void TakeDamge(int damge, int stunNumber)
    {
        Hp -= damge;
        stunResistance -= stunNumber;
        timeCD = 0;
        if (stunResistance <= 0)
        {
            aim.SetTrigger("Hit");
        }
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
    }
    void Death()
    {
        Destroy(me);
    }
}
