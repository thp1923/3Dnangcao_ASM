using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamge : MonoBehaviour
{
    public int HpMax;
    int Hp;
    // Start is called before the first frame update
    void Start()
    {
        Hp = HpMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamge(int damge)
    {
        Hp -= damge;
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
