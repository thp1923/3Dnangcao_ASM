using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackEffect : MonoBehaviour
{
    public GameObject FireBall;
    public Transform fireBallPoint;

    public GameObject[] fireDragonEffect;

    public GameObject[] dragonLightning;

    public float lightningRate = 1f;

    public ParticleSystem[] dragonBreath;

    public ParticleSystem dragonClaw;

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
        }
        else
        {
            foreach (var d in dragonBreath)
            {
                d.Play();
            }
        }
    }
}
