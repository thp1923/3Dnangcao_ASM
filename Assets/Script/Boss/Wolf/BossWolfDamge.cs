using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossWolfDamge : MonoBehaviour
{
    public Transform pointAttack;
    public Transform pointAttack2;
    public float radius;
    public float radius2;
    public int stunDamge;
    public float knockBack;
    public LayerMask attackMask;
    public int damge1;
    public int damge2;
    int Damge;

    public GameObject Shock;
    public GameObject Tornado;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damge1()
    {
        Damge = damge1;
        Attack();
    }
    public void Damge2()
    {
        Damge = damge2;
        Attack();
        GameObject instance = Instantiate(Shock, pointAttack2.position, Quaternion.identity);
    }

    public void SummonTornado()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject instance = Instantiate(Tornado, transform.position 
            + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, UnityEngine.Random.Range(-5f, 5f)), Quaternion.identity);
    }
    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapSphere(pointAttack.position, radius, attackMask);
        foreach (Collider player in colInfo)
        {
            if (player.GetComponent<PlayerTakeDamge>().isBlock)
            {
                GetComponent<EnemyTakeDamge>().TakeDamge(0, 1000);
                return;
            }
            player.GetComponent<PlayerTakeDamge>().TakeDamge(Damge, stunDamge, knockBack);
        }
    }
    public void Attack2()
    {
        Collider[] colInfo = Physics.OverlapSphere(pointAttack2.position, radius2, attackMask);
        foreach (Collider player in colInfo)
        {
            if (player.GetComponent<PlayerTakeDamge>().isBlock)
            {
                GetComponent<EnemyTakeDamge>().TakeDamge(0, 1000);
                return;
            }
            player.GetComponent<EnemyTakeDamge>().TakeDamge(Damge, stunDamge);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointAttack.position, radius);
        Gizmos.DrawWireSphere(pointAttack2.position, radius2);
    }
}
