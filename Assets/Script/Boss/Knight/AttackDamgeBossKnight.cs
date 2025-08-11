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
        Collider[] colInfo = Physics.OverlapBox(
            pointAttack.position,
            attackRange * 0.5f, // Vì OverlapBox dùng nửa kích thước
            pointAttack.rotation,
            attackMask
        );
        foreach (Collider player in colInfo)
        {
            if (player.GetComponent<PlayerTakeDamge>().isBlock)
            {
                GetComponent<EnemyTakeDamge>().TakeDamge(0, 2000, 0);
            }
            player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, (stunDamge[attackNum] + stunDamgeBonus), 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (pointAttack == null) return;

        Gizmos.color = Color.blue;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(pointAttack.position, pointAttack.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, attackRange);
    }
}
