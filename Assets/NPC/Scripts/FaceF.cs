using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceF : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
    }
}
