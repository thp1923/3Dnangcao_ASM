using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatsManager;

public class DragonPetAttack : StatsAttack
{
    public Vector3 breathRange;

    public LayerMask attackMask;

    bool isBreath;

    public Transform breathPoint;

    public float breathRate;

    public AttackDamgePlayer damgePlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        BaseATK = damgePlayer.BaseATK;
    }

    public void Breath(int num)
    {
        if (num != 0)
        {
            isBreath = false;
        }
        else
        {
            isBreath = true;
            Attack(0);
            StartCoroutine(BreathAttack(0));
        }
    }

    IEnumerator BreathAttack(int attackNum)
    {
        Collider[] colInfo = Physics.OverlapBox(breathPoint.position, breathRange * 0.5f, breathPoint.rotation, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<EnemyTakeDamge>().TakeDamge(atk, stunDamge[attackNum], 0);
        }
        yield return new WaitForSeconds(breathRate);
        if (isBreath)
        {
            StartCoroutine(BreathAttack(0));
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
