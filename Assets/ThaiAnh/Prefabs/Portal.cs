using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Portall;
    // Start is called before the first frame update
    void Awake()
    {
        checkedd();
    }
    void Start()
    {
        checkedd();
    }

    // Update is called once per frame
    void Update()
    {
        checkedd();
    }
    public void checkedd()
    {
        if (Boss == null)
        {
            Portall.SetActive(true);
        }
        else
        {
            Portall.SetActive(false);
        }
    }
}
