using StatsManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyTakeDamge : StatsAlive
{
    public LayerMask playerMask;
    public float rangeShow = 70f;
    public GameObject HP_Bar;
    bool isShow;
    bool isDeath;

    Animator aim;
    public GameObject me;
    public TextMeshProUGUI damPopUp;

    public int Point;
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
        ShowAndHide();
        if(HP_Bar == null) return;
        if(isShow) HP_Bar.SetActive(true);
        else HP_Bar.SetActive(false);
    }

    void ShowAndHide()
    {
        Collider[] playerIn = Physics.OverlapSphere(gameObject.transform.position, rangeShow, playerMask);
        if(playerIn.Length > 0 )
        {
            isShow = true;
        }
        else
        {
            isShow = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, rangeShow);
    }

    public override void TakeDamge(int damge, int stunDamge, int trueDamge)
    {
        if (isDeath) return;
        switch (type)
        {
            case TypeTakeDamge.Only:
                base.TakeDamge(damge, stunDamge, trueDamge);  
                damPopUp.text = DamPopUp.ToString();
                StartCoroutine(DamgePopUp());
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
            isDeath = true;
        }
    }

    IEnumerator DamgePopUp()
    {
        yield return new WaitForSeconds(0.5f);
        damPopUp.text = null;
    }

    void Death()
    {
        FindObjectOfType<PlayerAim>().RemoveEnemy(gameObject);
        foreach (GameObject items in itemsDrop)
        {
            Instantiate(items, gameObject.transform.position + new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)), Quaternion.identity);
        }
        FindObjectOfType<UpgradeStats>().AddPoint(Point);
        if(me != null)
            Destroy(me);
    }
}
