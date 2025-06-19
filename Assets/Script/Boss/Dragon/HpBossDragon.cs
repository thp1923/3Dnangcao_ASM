using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatsManager;
using TMPro;
using UnityEngine.SceneManagement;

public class HpBossDragon : StatsAlive
{
    Animator aim;
    public GameObject me;
    public TextMeshProUGUI damPopUp;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        aim = GetComponent<Animator>();
    }

    public override void TakeDamge(int damge, int stunDamge, int trueDamge)
    {
        base.TakeDamge(damge, stunDamge, trueDamge);
        damPopUp.text = DamPopUp.ToString();
        StartCoroutine(DamgePopUp());
        if (currentHP <= MaxHP / 2)
        {
            GetComponent<DragonAttackEffect>().TransPhase();
        }
        if (currentHP <= 0)
        {
            aim.SetBool("IsDeath", true);
        }
    }

    IEnumerator DamgePopUp()
    {
        yield return new WaitForSeconds(0.5f);
        damPopUp.text = null;
    }

    public void Death()
    {
        if (me != null)
            Destroy(me);
    }
}
