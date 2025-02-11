using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{

    public Transform player;
    float y_camera;
    // Start is called before the first frame update
    void Start()
    {
        y_camera = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, y_camera, player.position.z);
        transform.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);
    }
}
