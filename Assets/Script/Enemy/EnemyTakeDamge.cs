using StatsManager;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamge : StatsAlive
{
    Animator aim;
    Audio audioE;
    public GameObject me;
    [Header("---------Items Drop-----------")]
    public List<GameObject> itemsDrop;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        aim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamge(int damge, int stunDamge, int trueDamge)
    {
        switch (type)
        {
            case TypeTakeDamge.Only:
                base.TakeDamge(damge, stunDamge, trueDamge);  
                if(stunDamge > StunResistance)
                {
                    aim.SetTrigger("Hit");
                }
                break;
            case TypeTakeDamge.Branch:
                GetComponent<BrandHpEnemy>().TakeDam(damge, stunDamge, trueDamge);
                break;
            default:
                break;
        }
        if(currentHP <= 0)
        {
            aim.SetBool("IsDeath", true);
        }
    }



    void Death()
    {
        FindObjectOfType<PlayerAim>().RemoveEnemy(gameObject);
        foreach (GameObject items in itemsDrop)
        {
            Instantiate(items, gameObject.transform.position + new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)), Quaternion.identity);
        }
        if(me != null)
            Destroy(me);
    }
}
