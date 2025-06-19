using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    public int nextSceneIndex = 3; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneLoaderr.Instance.LoadLevel(nextSceneIndex);
    }
}

