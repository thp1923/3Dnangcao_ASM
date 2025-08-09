using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfPet : MonoBehaviour
{
    public PlayerAim playerAim;
    Animator animator;
    GameObject ClosestEnemy;

    public float erodeInRate = 0.02f;
    public float erodeOutRate = 0.05f;
    public float erodeRefreshRate = 0.005f;
    public List<SkinnedMeshRenderer> objectsToErode = new List<SkinnedMeshRenderer>();


    [Header("---Range Check Attack---")]
    public float range;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(ErodeObjectsStart());
    }

    void Check()
    {
        Collider[] enemy = Physics.OverlapSphere(transform.position, range, layerMask);
        if(enemy.Length > 0 )
        {
            animator.SetTrigger("Pound");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Check();
    }

    void Aim()
    {
        // Nếu có enemy đang lock
        if (playerAim != null && playerAim.closestEnemy != null)
        {
            ClosestEnemy = playerAim.closestEnemy;

            Vector3 direction = ClosestEnemy.transform.position - transform.position;
            direction.y = 0f; // Xoay trên mặt phẳng

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
        // Nếu không có enemy → xoay theo hướng nhìn của nhân vật
        else if (playerAim != null)
        {
            Vector3 lookDirection = playerAim.transform.forward;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

    IEnumerator ErodeObjectsEnd()
    {
        // Phase 2: erode out (reappear)
        float t = 0f;
        while (t < 1f)
        {
            t += erodeOutRate;
            foreach (var renderer in objectsToErode)
            {
                renderer.material.SetFloat("_Erode", t);
            }
            yield return new WaitForSeconds(erodeRefreshRate);
        }

        animator.ResetTrigger("Pound");
        // Sau khi tất cả đã tan biến → tắt GameObject chủ
        gameObject.SetActive(false);
    }

    IEnumerator ErodeObjectsStart()
    {
        // Phase 1: erode in (disappear)
        for (int i = 0; i < objectsToErode.Count; i++)
        {
            float t = 1f;
            while (t > 0f)
            {
                t -= erodeInRate;
                objectsToErode[i].material.SetFloat("_Erode", t);
                yield return new WaitForSeconds(erodeRefreshRate);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
