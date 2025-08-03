using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatsManager;

public class ArtoriasAttackDamge : StatsAttack
{
    Animator animator;

    public bool isHeavy;
    public float atkHeavyBonus;
    public float stunHeavyBonus;
    public LayerMask attackMask;
    public int trueDamge;
    public int hitStun;
    public int stunParry;
    int hit;

    [Header("---Attack(0)---")]
    public float rangeAttack;

    [Header("---Combo(1)---")]
    public float rangeCombo;
    public Transform comboPoint;

    [Header("---Kick(2)---")]
    public Vector3 kickRange;
    public Transform kickPoint;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("NotStun", true);
    }

    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        AttackDamge(attackNumber);
    }

    void AttackDamge(int attackIndex)
    {
        switch (attackIndex)
        {
            case 0:
                Collider[] attackPlayer = Physics.OverlapSphere(transform.position, rangeAttack, attackMask);
                foreach(var player in attackPlayer)
                {
                    if (player.GetComponent<PlayerTakeDamge>().isBlock)
                    {
                        GetComponent<EnemyTakeDamge>().TakeDamge(0, stunParry, 0);
                        hit++;
                        if (hit >= hitStun)
                        {
                            animator.SetBool("NotStun", false);
                            hit -= hitStun;
                        }
                        return;
                    }
                    if (isHeavy)
                    {
                        player.GetComponent<PlayerTakeDamge>().TakeDamge((int)(atk * atkHeavyBonus), (int)(stunDamge[attackIndex] * stunHeavyBonus), trueDamge);
                        return;
                    }
                    player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, stunDamge[attackIndex], trueDamge);
                }
                break;
            case 1:
                Collider[] comboPlayer = Physics.OverlapSphere(comboPoint.position, rangeCombo, attackMask);
                foreach (var player in comboPlayer)
                {
                    if (player.GetComponent<PlayerTakeDamge>().isBlock)
                    {
                        GetComponent<EnemyTakeDamge>().TakeDamge(0, stunParry, 0);
                        hit++;
                        if (hit >= hitStun)
                        {
                            animator.SetBool("NotStun", false);
                            hit -= hitStun;
                        }
                        return;
                    }
                    if (isHeavy)
                    {
                        player.GetComponent<PlayerTakeDamge>().TakeDamge((int)(atk * atkHeavyBonus), (int)(stunDamge[attackIndex] * stunHeavyBonus), trueDamge);
                        return;
                    }
                    player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, stunDamge[attackIndex], trueDamge);
                }
                break;
            case 2:
                Collider[] kickPlayer = Physics.OverlapBox(kickPoint.position, kickRange, Quaternion.identity, attackMask);
                foreach (var player in kickPlayer)
                {
                    if (player.GetComponent<PlayerTakeDamge>().isBlock)
                    {
                        GetComponent<EnemyTakeDamge>().TakeDamge(0, stunParry, 0);
                        hit++;
                        if (hit >= hitStun)
                        {
                            animator.SetBool("NotStun", false);
                            hit -= hitStun;
                        }
                        return;
                    }
                    if (isHeavy)
                    {
                        player.GetComponent<PlayerTakeDamge>().TakeDamge((int)(atk * atkHeavyBonus), (int)(stunDamge[attackIndex] * stunHeavyBonus), trueDamge);
                        return;
                    }
                    player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, stunDamge[attackIndex], trueDamge);
                }
                break;
            default:
                break;
        }
        
    }

    public void StunBuff(int index)
    {
        if(index != 0)
        {
            isHeavy = true;
        }
        else
        {
            isHeavy = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(comboPoint.position, rangeCombo);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(kickPoint.position, kickRange);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
