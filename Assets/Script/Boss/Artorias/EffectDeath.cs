using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeath : MonoBehaviour
{
    public ParticleSystem dieEffect;
    void Start()
    {
        dieEffect.Stop();
    }

    public void Die()
    {
        dieEffect.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
