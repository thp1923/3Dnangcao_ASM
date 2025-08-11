using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFireWolfPet : MonoBehaviour
{
    public ParticleSystem firePound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireEnd()
    {
        StartCoroutine(EndFire());
    }

    public IEnumerator EndFire()
    {
        yield return new WaitForSecondsRealtime(3f);

        if (firePound != null)
        {
            firePound.Stop();
        }
    }
}
