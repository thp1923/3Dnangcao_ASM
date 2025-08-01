using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public GameObject parentObject;
    public List<Transform> childTransforms = new List<Transform>();

    void Start()
    {
        AddAllChildTransforms(parentObject);
    }

    void AddAllChildTransforms(GameObject parent)
    {
        childTransforms.Clear(); // Xoá cũ nếu cần
        foreach (Transform child in parent.transform)
        {
            childTransforms.Add(child);
        }
    }
}
