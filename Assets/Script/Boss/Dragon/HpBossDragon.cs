using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatsManager;

public class HpBossDragon : StatsAlive
{
    Animator aim;
    public GameObject me;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        aim = GetComponent<Animator>();
    }

    public override void TakeDamge(int damge, int stunDamge, int trueDamge)
    {
        base.TakeDamge(damge, stunDamge, trueDamge);
        if(currentHP <= 0)
        {
            aim.SetBool("IsDeath", true);
        }
    }

    public void Death()
    {
        if(me != null)
            Destroy(me);
    }
}
