using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnightEffect : MonoBehaviour
{
    public ParticleSystem Fire;
    public Transform firePoint;
    public AudioSource source;
    public AudioClip clip;

    public GameObject FireExplosion;
    void Start()
    {
        Fire.Stop();
        Fire.GetComponentInChildren<Light>().enabled = false;
        FireExplosion.SetActive(false);
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
        source.PlayOneShot(clip);
        StartCoroutine(FireOff());
    }

    IEnumerator FireOff()
    {
        yield return new WaitForSeconds(4f);
        Fire.Stop();
        Fire.GetComponentInChildren<Light>().enabled = false;
    }

    public void ExplosionFire()
    {
        FireExplosion.SetActive(true);
    }
}
