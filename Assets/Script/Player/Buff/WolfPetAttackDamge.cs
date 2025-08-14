using StatsManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfPetAttackDamge : StatsAttack
{
    public AttackDamgePlayer damgePlayer;
    public float attackRange;
    public LayerMask attackMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        BaseATK = damgePlayer.BaseATK;
    }

    public override void Attack(int attackNumber)
    {
        base.Attack(attackNumber);
        Attack1(attackNumber);
    }

    public void Attack1(int attackNum)
    {
        Collider[] colInfo = Physics.OverlapSphere(gameObject.transform.position, attackRange, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<EnemyTakeDamge>().TakeDamge(atk, (stunDamge[attackNum] + stunDamgeBonus), 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, attackRange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
