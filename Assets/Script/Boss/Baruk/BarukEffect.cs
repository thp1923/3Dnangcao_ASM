using System.Collections;
using UnityEngine;

public class BarukEffect : MonoBehaviour
{
    public ParticleSystem[] fireHowls;
    public ParticleSystem firePound;

    private void Start()
    {
        if (firePound != null)
        {
            firePound.Stop();
            Light poundLight = firePound.GetComponentInChildren<Light>();
            if (poundLight != null) poundLight.enabled = false;
        }

        if (fireHowls != null)
        {
            foreach (var fireHowl in fireHowls)
            {
                if (fireHowl != null)
                {
                    fireHowl.Stop();
                    Light light = fireHowl.GetComponent<Light>();
                    if (light != null) light.enabled = false;
                }
                else
                {
                    Debug.LogWarning("Một phần tử trong fireHowls bị null.");
                }
            }
        }
    }


    public void Howl(int index)
    {
        foreach (var fireHowl in fireHowls)
        {
            if (fireHowl != null)
            {
                Light light = fireHowl.GetComponent<Light>();
                if (index == 0)
                {
                    fireHowl.Stop();
                    if (light != null) light.enabled = false;
                }
                else
                {
                    fireHowl.Play();
                    if (light != null) light.enabled = true;
                }
            }
        }
    }

    public void Pound()
    {
        if (firePound != null)
        {
            firePound.Play();
            Light poundLight = firePound.GetComponentInChildren<Light>();
            if (poundLight != null) poundLight.enabled = true;

            firePound.transform.position = new Vector3(
                transform.position.x,
                firePound.transform.position.y,
                transform.position.z
            );

            StartCoroutine(EndFire());
        }
        else
        {
            Debug.LogWarning("firePound is null in Pound().");
        }
    }

    private IEnumerator EndFire()
    {
        yield return new WaitForSecondsRealtime(3f);

        if (firePound != null)
        {
            firePound.Stop();
            Light poundLight = firePound.GetComponentInChildren<Light>();
            if (poundLight != null) poundLight.enabled = false;
        }
    }
}
