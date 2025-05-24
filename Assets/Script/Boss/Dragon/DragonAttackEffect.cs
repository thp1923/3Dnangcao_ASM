using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackEffect : MonoBehaviour
{
    public AudioSource sourceSFX;
    public AudioSource sourceGoar;
    [Header("------------Fire Ball------------")]
    public GameObject FireBall;
    public Transform fireBallPoint;
    public AudioClip fireBallClip;

    [Header("------------Trans Form------------")]
    public GameObject[] fireDragonEffect;

    [Header("------------Lightning------------")]
    public GameObject[] dragonLightning;
    public AudioClip lightningClip;

    public float lightningRate = 1f;

    [Header("------------Breath------------")]
    public ParticleSystem[] dragonBreath;
    public AudioClip breathSound;

    [Header("------------Claw------------")]
    public ParticleSystem dragonClaw;

    [Header("------------Goar------------")]
    public AudioClip[] dragonGoar;

    private void Start()
    {
        dragonClaw.Stop();
        foreach(var f in fireDragonEffect)
        {
            f.SetActive(false);
        }
        foreach(var d in dragonBreath)
        {
            d.Stop();
        }
        foreach (var l in dragonLightning)
        {
            l.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TransPhase();
        }
    }

    public void FlyBreath()
    {
        Instantiate(FireBall, fireBallPoint.transform.position, Quaternion.identity);
        sourceSFX.PlayOneShot(fireBallClip);
    }

    public void TransPhase()
    {
        foreach (var f in fireDragonEffect)
        {
            f.SetActive(true);
        }
    }

    public void DragonLightning()
    {
        StartCoroutine(Lightning());
    }

    IEnumerator Lightning()
    {
        foreach (var l in dragonLightning)
        {
            l.SetActive(true);
            sourceSFX.PlayOneShot(lightningClip);
            yield return new WaitForSeconds(lightningRate);
        }
    }

    public void DragonClaw(int num)
    {
        if (num != 0)
        {
            dragonClaw.Stop();
        }
        else
        {
            dragonClaw.Play();
        }
    }

    public void DragonBreath(int num)
    {
        if(num != 0)
        {
            foreach (var d in dragonBreath)
            {
                d.Stop();
            }
            sourceSFX.Stop();
        }
        else
        {
            foreach (var d in dragonBreath)
            {
                d.Play();
            }
            sourceSFX.PlayOneShot(breathSound);
        }
    }

    public void StartGoar(int goarNum)
    {
        sourceGoar.PlayOneShot(dragonGoar[goarNum]);
    }
    public void StopGoar()
    {
        sourceGoar.Stop(); 
    }
}
