using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Aim")]
    List<GameObject> enemiesList = new List<GameObject>();
    GameObject closestEnemy;
    public float aimRange;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemiesInScene)
        {
            enemiesList.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClosestEnemy()
    {
        float range = aimRange;
        foreach (GameObject enemyGameObject in enemiesList)
        {
            if(enemyGameObject == null) return;
            float dist = Vector3.Distance(enemyGameObject.transform.position, transform.position);
            if (dist < range)
            {
                range = dist;
                closestEnemy = enemyGameObject;
                Vector3 enemyPosition = new Vector3(closestEnemy.transform.position.x, 0, closestEnemy.transform.position.z);
                gameObject.transform.LookAt(enemyPosition);
            }
        }
    }

    public void RemoveEnemy()
    {
        if (closestEnemy != null)
        {
            enemiesList.Remove(closestEnemy);
        }
    }
}
