using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    CapsuleCollider cp;
    float radius;
    float height;
    Vector3 center;
    public float radius1;
    public float height1;
    public Vector3 center1;
    public float radius2;
    public Vector3 center2;
    public float height2;
    // Start is called before the first frame update
    void Start()
    {
        cp = GetComponent<CapsuleCollider>();
        radius = cp.radius;
        center = cp.center;
        height = cp.height;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack1()
    {
        cp.center = center1;
        cp.radius = radius1;
        cp.height = height1;
    }
    public void Attack2()
    {
        cp.center = center2;
        cp.radius = radius2;
        cp.height = height2;
    }

    public void ResetCp()
    {
        cp.center = center;
        cp.radius = radius;
        cp.height = height;
    }
}
