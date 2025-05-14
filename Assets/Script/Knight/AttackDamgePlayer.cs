using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackDamgePlayer : MonoBehaviour
{
    public Transform pointAttack2;
    public Vector3 box;
    public float radius;
    public int stunDamge;
    public LayerMask attackMask;
    public int damge1;
    public int damge2;
    public int damge3;
    int Damge;
    int DamgeBonous;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Bonus(int damgeBonus)
    {
        DamgeBonous += damgeBonus;
    }
    
    public void Damge2()
    {
        Damge = damge2 + DamgeBonous;
        Attack2();
    }
    public void Damge3()
    {
        Damge = damge3 + DamgeBonous;
        Attack2();
    }
    public void Attack2()
    {
        Collider[] colInfo = Physics.OverlapBox(pointAttack2.position, box, Quaternion.identity, attackMask);
        foreach (Collider enemy in colInfo)
        {
            enemy.GetComponent<EnemyTakeDamge>().TakeDamge(Damge, stunDamge, 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pointAttack2.position, box);
    }
}
