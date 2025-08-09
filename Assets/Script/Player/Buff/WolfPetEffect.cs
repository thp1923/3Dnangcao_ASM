using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfPetEffect : MonoBehaviour
{
    public ParticleSystem firePound;

    // Start is called before the first frame update
    void Start()
    {
        if (firePound != null)
        {
            firePound.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pound()
    {
        if (firePound != null)
        {
            firePound.Play();

            firePound.transform.position = new Vector3(
                transform.position.x,
                firePound.transform.position.y,
                transform.position.z
            );

            firePound.GetComponent<GreenFireWolfPet>().FireEnd();
        }
    }
}
