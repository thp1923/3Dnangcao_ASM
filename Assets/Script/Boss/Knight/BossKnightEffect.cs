using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnightEffect : MonoBehaviour
{
    public ParticleSystem Fire;
    public Transform firePoint;
    void Start()
    {
        Fire.Stop();
        Fire.GetComponentInChildren<Light>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireInstance()
    {
        Fire.gameObject.transform.position = new Vector3(firePoint.position.x, Fire.gameObject.transform.position.y, firePoint.position.z);
        Fire.Play();
        Fire.GetComponentInChildren<Light>().enabled = true;
        StartCoroutine(FireOff());
    }

    IEnumerator FireOff()
    {
        yield return new WaitForSeconds(4f);
        Fire.Stop();
        Fire.GetComponentInChildren<Light>().enabled = false;
    }
}
