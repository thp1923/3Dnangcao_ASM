using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonRotation : MonoBehaviour
{
    Transform player;
    public float rotationSpeed = 5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Tính góc giữa hướng nhìn và hướng tới player
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // Tính quaternion quay về phía player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
