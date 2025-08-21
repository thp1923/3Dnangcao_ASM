using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorBossDie : MonoBehaviour
{
    public GameObject Boss;

    public GameObject Door;

    private void Start()
    {
        if(Door != null) Door.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Boss == null)
        {
            if (Door != null)
                Door.SetActive(true);
            Destroy(gameObject);
        }
    }
}
