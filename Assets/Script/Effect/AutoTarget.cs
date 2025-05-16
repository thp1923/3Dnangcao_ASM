using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTarget : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        gameObject.transform.position = target.position;
    }
}
