using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonefine : MonoBehaviour
{
    public float range;
    public LayerMask mask;

    bool isHere;
    LockController lockController;
    // Start is called before the first frame update
    void Start()
    {
        lockController = FindObjectOfType<LockController>();
    }

    // Update is called once per frame
    void Update()
    {
        Check();
        if(isHere && Input.GetKeyDown(KeyCode.F) && !lockController.isInven && !lockController.isQuest)
        {
            FindAnyObjectByType<UpgradeStats>().CanvaStats(false);
        }
    }
    void Check()
    {
        Collider[] check = Physics.OverlapSphere(transform.position, range, mask);
        if(check.Length > 0)
        {
            isHere = true;
        }
        else
        {
            isHere = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
