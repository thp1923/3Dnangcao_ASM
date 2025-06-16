using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPhu : MonoBehaviour
{
    public Transform camMain;

    // Update is called once per frame
    void Update()
    {
        if(camMain != null)
        {
            transform.position = camMain.transform.position;
            transform.rotation = camMain.transform.rotation;
        }
    }
}
