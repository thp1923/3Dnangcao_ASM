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
            Explosion.transform.position = new Vector3(gameObject.transform.position.x, Explosion.transform.position.y, gameObject.transform.position.z);
            Explosion.SetActive(true);
            StartCoroutine(Hide());
        }
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
