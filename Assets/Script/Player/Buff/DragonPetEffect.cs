using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonPetEffect : MonoBehaviour
{
    [Header("------------Breath------------")]
    public ParticleSystem[] dragonBreath;
    public AudioSource breathSource;
    public AudioClip breathSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        foreach (var d in dragonBreath)
        {
            d.Stop();
        }
    }

    public void DragonBreath(int num)
    {
        if (num != 0)
        {
            foreach (var d in dragonBreath)
            {
                d.Stop();
            }
            breathSource.Stop();
        }
        else
        {
            foreach (var d in dragonBreath)
            {
                d.Play();
            }
            breathSource.PlayOneShot(breathSound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
