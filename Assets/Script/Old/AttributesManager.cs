using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class AttributesManager : MonoBehaviour
{
    public int Health;
    public int Attack;
    public Slider HpBar;

    public int armor;

    public float critDamge = 1.5f;
    public float critRate = 0.5f;
    void Start()
    {
        HpBar.maxValue = Health;
        HpBar.value = Health;
    }
    

    public void TakeDamge(int amount)
    {
        Health -= amount - (amount * armor/100);
        HpBar.value = Health;
    }

    //gây damge cho target
    public void DealDamge(GameObject target)
    {
        // lấy AttributesManager của target truyền vào
        var atm = target.GetComponent<AttributesManager>();
        if (atm != null)
        {
            float totalDamge = Attack;
            if(Random.Range(0f, 1f) < critRate)
            {
                totalDamge *= critDamge;
            }
            int damage = (int)totalDamge;
            atm.TakeDamge(damage);
        }
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
