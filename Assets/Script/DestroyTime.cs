using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    [SerializeField] float timeP = 0.3f;
    private void OnEnable()
    {
        Destroy(this.gameObject, timeP);
    }
}
