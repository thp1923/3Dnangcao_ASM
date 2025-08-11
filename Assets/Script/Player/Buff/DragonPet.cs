using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonPet : MonoBehaviour
{
    public PlayerAim playerAim;
    Animator animator;
    GameObject ClosestEnemy;

    public float erodeInRate = 0.03f;
    public float erodeOutRate = 0.03f;
    public float erodeRefreshRate = 0.015f;
    public List<SkinnedMeshRenderer> objectsToErode = new List<SkinnedMeshRenderer>();


    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(ErodeObjectsStart());
    }


    // Update is called once per frame
    void Update()
    {
        Aim();
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

}
