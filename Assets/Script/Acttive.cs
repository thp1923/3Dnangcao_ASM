using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acttive : MonoBehaviour
{
    public GameObject obj;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(obj == null)
        {
            target.SetActive(true);
        }
        else
        {
            target.SetActive(false);
        }
    }
}
