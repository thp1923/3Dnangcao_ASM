using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrandHpEnemy : MonoBehaviour
{
    public HpBossDragon hpBossDragon;

    public void TakeDam(int Damge, int stunDamge, int trueDamge)
    {
        if (hpBossDragon != null)
        {
            hpBossDragon.TakeDamge(Damge, stunDamge, trueDamge);
        }
    }
}
