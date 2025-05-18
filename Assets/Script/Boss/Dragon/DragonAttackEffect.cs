using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackEffect : MonoBehaviour
{
    public GameObject FireBall;
    public Transform fireBallPoint;

    public GameObject[] fireDragonEffect;

    public ParticleSystem[] dragonBreath;

    private void Start()
    {
        foreach(var f in fireDragonEffect)
        {
            f.SetActive(false);
        }
        foreach(var d in dragonBreath)
        {
            d.Stop();
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
