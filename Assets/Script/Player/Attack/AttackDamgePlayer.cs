using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using StatsManager;

public class AttackDamgePlayer : StatsAttack
{
    public Transform pointAttack2;
    public Vector3 attackRange;
    public LayerMask attackMask;

    public int godDamge;
    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        Attack1(attackNumber);
    }

    public void Attack1(int attackNum)
    {
        Collider[] colInfo = Physics.OverlapBox(
            pointAttack2.position,
            attackRange * 0.5f, // Vì OverlapBox dùng nửa kích thước
            pointAttack2.rotation,
            attackMask
        );
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<EnemyTakeDamge>().TakeDamge(atk, (stunDamge[attackNum]+stunDamgeBonus), godDamge);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (pointAttack2 == null) return;

        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(pointAttack2.position, pointAttack2.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, attackRange);
    }
}
