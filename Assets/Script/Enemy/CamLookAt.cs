using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLookAt : MonoBehaviour
{
    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
