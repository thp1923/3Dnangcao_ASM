using UnityEngine;
using StatsManager;
using System.Collections;

public class DragonAttack : StatsAttack
{
    public Transform[] attackPoints;
    public Transform breathPoint;

    public float[] radius;
    public Vector3 breathRange;

    public LayerMask attackMask;

    bool isBreath;

    public float breathRate;

    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        if(attackNumber < 4)
        {
            BaseAttack(attackNumber);
        }
        else
        {
            Breath(0);
        }
    }

    public void BaseAttack(int num)
    {
        Collider[] colInfo = Physics.OverlapSphere(attackPoints[num].position, radius[num], attackMask);
        foreach (Collider player in colInfo)
        {
            player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, stunDamge[num], 0);
        }
    }

    public void Breath(int num)
    {
        if(num != 0)
        {
            isBreath = false;
        }
        else
        {
            isBreath = true;
            StartCoroutine(BreathAttack(stunDamge.Length - 1));
        }
    }

    IEnumerator BreathAttack(int attackNum)
    {
        Collider[] colInfo = Physics.OverlapBox(breathPoint.position, breathRange * 0.5f, breathPoint.rotation, attackMask);
        foreach (Collider player in colInfo)
        {
            player.GetComponent<PlayerTakeDamge>().TakeDamge(atk, stunDamge[attackNum], 0);
        }
        yield return new WaitForSeconds(breathRate);
        if (isBreath)
        {
            StartCoroutine(BreathAttack(stunDamge.Length - 1));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < attackPoints.Length && i < radius.Length; i++)
        {
            if (attackPoints[i] != null)
            {
                Gizmos.DrawWireSphere(attackPoints[i].position, radius[i]);
            }
        }

        if (breathPoint != null)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                breathPoint.position,
                breathPoint.rotation,
                Vector3.one
            );

            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, breathRange);
        }

        // Reset matrix sau khi vẽ
        Gizmos.matrix = Matrix4x4.identity;
    }
}
