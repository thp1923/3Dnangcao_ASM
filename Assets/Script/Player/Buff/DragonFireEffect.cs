using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireEffect : MonoBehaviour
{
    public Transform target;         
    public float moveSpeed = 5f;       

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );
    }

}
