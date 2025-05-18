using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public string tagCompare;
    public GameObject Explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagCompare))
        {
            GameObject instance = GameObject.Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject, 2f);
        }
    }
}
