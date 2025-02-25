using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoFollow : MonoBehaviour
{
    public GameObject GameObjectTornado;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = GameObjectTornado.transform.position;
    }
}
