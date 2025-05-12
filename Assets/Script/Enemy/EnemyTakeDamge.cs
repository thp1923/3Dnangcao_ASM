using StatsManager;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamge : StatsAlive
{
    Animator aim;
    public GameObject me;
    Audio audioE;

    [Header("Time")]

    public Transform HitPoint;

    public GameObject HitEffect;

    [Header("---------Items Drop-----------")]
    public List<GameObject> itemsDrop;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        aim = GetComponent<Animator>();
        audioE = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamge(int damge, int stunDamge, int trueDamge)
    {
        base.TakeDamge(damge, stunDamge, trueDamge);
        if(currentHP <= 0)
        {
            switch(type)
            {
                case TypeTakeDamge.Only:
                    Death();
                    break;
                case TypeTakeDamge.Branch:
                    Debug.Log("T? t?");
                    break;
                default:
                    break;
            }
        }
    }



    void Death()
    {
        FindObjectOfType<PlayerAim>().RemoveEnemy();
        foreach(GameObject items in itemsDrop)
        {
            Instantiate(items, gameObject.transform.position + new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)), Quaternion.identity);
        }
        Destroy(me);
    }
}
