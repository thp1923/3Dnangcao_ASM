using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_soi_die_mo_cong : MonoBehaviour
{
    public GameObject Boss;
    public GameObject cong;
    private EnemyTakeDamge enemyTakeDamage;

    private void Start()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyTakeDamage = enemy.GetComponent<EnemyTakeDamge>();
    }

    private void Update()
    {
        if (enemyTakeDamage.currentHP <= 0)
        {
            cong.SetActive(false);
        }
    }
}
