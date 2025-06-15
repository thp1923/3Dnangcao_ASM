using UnityEngine;
using StatsManager;

public class AttackDamgeBossKnight : StatsAttack
{
    public Transform pointAttack;
    public Vector3 attackRange;
    public LayerMask attackMask;

    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        Attack1(attackNumber);
    }

    public void Attack1(int attackNum)
    {
        Collider[] colInfo = Physics.OverlapBox(pointAttack.position, attackRange, Quaternion.identity, attackMask);
        foreach (Collider player in colInfo)
        {
            player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, (stunDamge[attackNum] + stunDamgeBonus), 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pointAttack.position, attackRange);
    }
}
