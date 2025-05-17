using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using StatsManager;

public class AttackDamgePlayer : StatsAttack
{
    public Transform pointAttack2;
    public Vector3 box;
    public LayerMask attackMask;

    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        Attack1();
    }

    public void Attack1()
    {
        Collider[] colInfo = Physics.OverlapBox(pointAttack2.position, box, Quaternion.identity, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<EnemyTakeDamge>().TakeDamge(atk, stunDamge, 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pointAttack2.position, box);
    }
}
