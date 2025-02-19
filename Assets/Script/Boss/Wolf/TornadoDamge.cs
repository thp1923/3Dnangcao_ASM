using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoDamge : MonoBehaviour
{
    public LayerMask attackMask;
    public Vector3 box;
    public Transform point;
    public int damge1;
    public int stunDamge;
    public int knockBack;
    int Damge;

    public string tagAttack;

    public int attackCount;
    public int attackMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(attackCount >= attackMax)
        {
            Destroy(gameObject);
        }
    }

    public void Damge1()
    {
        Damge = damge1;
        Attack();
    }

    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapBox(point.position, box, Quaternion.identity, attackMask);
        foreach (Collider player in colInfo)
        {
            attackCount++;
            Debug.Log("attack" + attackCount);
            if (player.GetComponent<PlayerTakeDamge>().isBlock || player.GetComponent<PlayerTakeDamge>().isDeath)
            {
                Destroy(gameObject);
                return;
            }
            player.GetComponent<PlayerTakeDamge>().TakeDamge(Damge, stunDamge, knockBack);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(point.position, box);
    }
}
