using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class BarukEffect : MonoBehaviour
{
    public GameObject[] fireHowls;

    public GameObject firePound;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject fireHowl in fireHowls)
        {
            fireHowl.SetActive(false);
        }
        firePound.SetActive(false);
    }

    public void Howl(int index)
    {
        if(index == 0)
        {
            foreach (GameObject fireHowl in fireHowls)
            {
                fireHowl.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject fireHowl in fireHowls)
            {
                fireHowl.SetActive(true);
            }
        }
    }

    public void Pound()
    {
        firePound.SetActive(true);
        firePound.transform.position = new Vector3(gameObject.transform.position.x, firePound.transform.position.y, gameObject.transform.position.z);
        StartCoroutine(EndFire());
    }

    IEnumerator EndFire()
    {
        yield return new WaitForSecondsRealtime(3f);
        firePound.SetActive(false);
    }
}
