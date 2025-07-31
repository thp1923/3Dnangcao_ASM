using StatsManager;
using System.Dynamic;
using UnityEngine;

public class BarukAttackDamge : StatsAttack
{
    public Transform pointAttack1;
    public Transform pointAttack2;
    public Transform pointAttack3;
    public Vector3 attackRange;
    public Vector3 attackRange2;
    public float attackRange3;
    public LayerMask attackMask;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        if(attackNumber >= (ATK.Count - 1))
        {
            Attack2(ATK.Count - 1);
            return;
        }
        else if(attackNumber >= (ATK.Count - 2))
        {
            Attack2(ATK.Count - 2);
            return;
        }
        Attack1(attackNumber);
    }

    public void Attack1(int attackNum)
    {
        Collider[] colInfo = Physics.OverlapBox(pointAttack1.position, attackRange, Quaternion.identity, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<PlayerTakeDamge>().TakeDamge(atk, (stunDamge[attackNum] + stunDamgeBonus), 0);
        }
    }
    public void Attack2(int attackNum)
    {
        Collider[] colInfo = Physics.OverlapBox(pointAttack2.position, attackRange2, Quaternion.identity, attackMask);
        foreach (Collider enemy in colInfo)
        {
            if(attackNum == 5)
                animator.SetTrigger("Combo3");
            enemy.GetComponent<PlayerTakeDamge>().TakeDamge(atk, (stunDamge[attackNum] + stunDamgeBonus), 0);
        }
    }
    public void Attack3(int attackNum)
    {
        Collider[] colInfo = Physics.OverlapSphere(pointAttack3.position, attackRange3, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<PlayerTakeDamge>().TakeDamge(atk, (stunDamge[attackNum] + stunDamgeBonus), 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pointAttack1.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pointAttack2.position, attackRange2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointAttack3.position, attackRange3);
    }
}
