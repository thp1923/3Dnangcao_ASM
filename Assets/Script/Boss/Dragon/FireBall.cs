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
            GameObject instance = GameObject.Instantiate(Explosion, 
                new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z), 
                Quaternion.Euler(-90, 0, 0));
            Destroy(gameObject, 2f);
        }
    }
}
