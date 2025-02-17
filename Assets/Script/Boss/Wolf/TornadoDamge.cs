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

    }

    public void Attack()
    {
        Collider[] colInfo = Physics.OverlapBox(point.position, box, Quaternion.identity, attackMask);
        foreach (Collider player in colInfo)
        {
            if (player.GetComponent<PlayerTakeDamge>().isBlock)
            {
                Destroy(gameObject);
                return;
            }
            player.GetComponent<PlayerTakeDamge>().TakeDamge(Damge, stunDamge, knockBack);
        }
    }
}
