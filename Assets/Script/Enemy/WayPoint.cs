using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public GameObject wayPoint;
    public List<Transform> WayPoints = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObject = wayPoint;

        foreach (Transform child in parentObject.transform)
        {
            WayPoints.Add(child.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
