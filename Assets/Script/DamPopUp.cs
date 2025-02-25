using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DamPopUp : MonoBehaviour
{
    public GameObject me;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Delete()
    {
        Destroy(me);
    }
}
